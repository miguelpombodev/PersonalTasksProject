using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PersonalTasksProject.Business.Implementations;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.Configuration.Settings;
using PersonalTasksProject.Context;
using PersonalTasksProject.DTOs.Mappings;
using PersonalTasksProject.Providers;
using PersonalTasksProject.Providers.Interfaces;
using PersonalTasksProject.Repositories.Implementations;
using PersonalTasksProject.Repositories.Interfaces;
using Serilog;

namespace PersonalTasksProject.Configuration;

public class AppConfiguration : IAppConfiguration
{
    private static AppConfiguration _instance;

    private static readonly object _lock = new object();

    public static AppConfiguration Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new AppConfiguration();
                }
            }

            return _instance;
        }
    }

    public string PostgresConnection { get; set; }
    public SMTPConfigurationSettings Smtp { get; set; } = new();
    public JwtConfigurationSettings Jwt { get; set; } = new();
    public AwsConfigurationSettings Aws { get; set; } = new();
    public Dictionary<string, string> EmailBodies { get; set; }

    private static string GetAndValidateConfigurationValue(WebApplicationBuilder builder, string key,
        bool isConnectionString = false)
    {
        var value = isConnectionString ? builder.Configuration.GetConnectionString(key) : builder.Configuration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new NullReferenceException($"Please provide a proper value to - {key}");
        }

        return value;
    }

    private static IConfigurationSection GetAndValidateConfigurationSection(WebApplicationBuilder builder, string key)
    {
        var section = builder.Configuration.GetSection(key);

        foreach (var value in section.GetChildren().ToList())
        {
            if (string.IsNullOrWhiteSpace(value.Value))
            {
                throw new NullReferenceException($"Please provide a proper value to - {value.Key} in {key}");
            }
        }

        return section;
    }

    public static void BuildAppConfigurations(WebApplicationBuilder builder)
    {
        var appConfig = AppConfiguration.Instance;

        appConfig.PostgresConnection = GetAndValidateConfigurationValue(builder, "MainDatabaseCS", true);
        var smtp = new SMTPConfigurationSettings();
        GetAndValidateConfigurationSection(builder, "SmtpMailTrap").Bind(smtp);
        appConfig.Smtp = smtp;

        var jwt = new JwtConfigurationSettings();
        GetAndValidateConfigurationSection(builder, "Jwt").Bind(jwt);
        appConfig.Jwt = jwt;

        var aws = new AwsConfigurationSettings();
        GetAndValidateConfigurationSection(builder, "AWSCredentials").Bind(aws);
        appConfig.Aws = aws;

        appConfig.EmailBodies.Add("Create Task Email", builder.Configuration.GetValue<string>("CreatedTaskEmailBody"));

        builder.Services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(
                appConfig.PostgresConnection
            )
        );
        builder.Services.AddSingleton(appConfig);
    }

    public static void BuildAppLogger(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((hostContext, services, configuration) =>
        {
            configuration.WriteTo.Console().CreateLogger();
        });
    }

    public static void BuildAppServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider =>
            provider.GetRequiredService<AppDbContext>());

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITasksService, TasksService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITasksRepository, TasksRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddAutoMapper(typeof(CreateUserDtoMappingProfile));
        builder.Services.AddAutoMapper(typeof(CreateTaskDtoMappingProfile));

        builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
        builder.Services.AddSingleton<SmtpEmailProvider>();
        builder.Services.AddSingleton<FileProvider>();
    }

    public static void BuildSwaggerService(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(o =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT Token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
            };

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

            o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            o.AddSecurityRequirement(securityRequirement);
        });
    }

    public static void BuildAwsServices(WebApplicationBuilder builder)
    {
        var appConfig = AppConfiguration.Instance;
        var credentials = new BasicAWSCredentials(appConfig.Aws.AwsAccessKeyId, appConfig.Aws.AwsSecretAccessKey);

        var s3Config = new AmazonS3Config()
        {
            RegionEndpoint = RegionEndpoint.SAEast1
        };

        builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(credentials, s3Config));
    }
}