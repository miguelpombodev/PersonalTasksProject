using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PersonalTasksProject.Business.Implementations;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.Configuration;
using PersonalTasksProject.Configuration.Settings;
using PersonalTasksProject.Context;
using PersonalTasksProject.DTOs.Mappings;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Extensions;
using PersonalTasksProject.Middlewares;
using PersonalTasksProject.Providers;
using PersonalTasksProject.Providers.Interfaces;
using PersonalTasksProject.Repositories.Implementations;
using PersonalTasksProject.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

BuildAppConfigurations();

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();

BuildSwaggerService(builder);

BuildAppServices(builder);

builder.Services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
        {
            var appConfig = AppConfiguration.Instance;
            option.SaveToken = true;
            option.RequireHttpsMetadata = false;
            option.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.Jwt.Secret)),
                ValidateIssuer = true,
                ValidIssuer = appConfig.Jwt.Issuer,
                ValidateAudience = true,
                ValidAudience = appConfig.Jwt.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        }
    );

BuildAwsServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ApplyMigrationsAsync();

app.UseHttpsRedirection();

app.UseExceptionHandler();
app.MapControllers();
app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.Run();

string GetAndValidateConfigurationValue(string key, bool isConnectionString = false)
{
    var value = isConnectionString ? builder.Configuration.GetConnectionString(key) : builder.Configuration[key];
        
    if (string.IsNullOrWhiteSpace(value))
    {
        throw new NullReferenceException($"Please provide a proper value to - {key}");
    }
        
    return value;
}

IConfigurationSection GetAndValidateConfigurationSection(string key)
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

void BuildAppConfigurations()
{
    var appConfig = AppConfiguration.Instance;
    
    appConfig.PostgresConnection = GetAndValidateConfigurationValue("MainDatabaseCS", true);
    var smtp = new SMTPConfigurationSettings();
    GetAndValidateConfigurationSection("SmtpMailTrap").Bind(smtp);
    appConfig.Smtp = smtp;

    var jwt = new JwtConfigurationSettings();
    GetAndValidateConfigurationSection("Jwt").Bind(jwt);
    appConfig.Jwt = jwt;

    var aws = new AwsConfigurationSettings();
    GetAndValidateConfigurationSection("AWSCredentials").Bind(aws);
    appConfig.Aws = aws;
    
    appConfig.EmailBodies.Add("Create Task Email", builder.Configuration.GetValue<string>("CreatedTaskEmailBody"));
    
    builder.Services.AddDbContext<AppDbContext>(
        options => options.UseNpgsql(
            appConfig.PostgresConnection
        )
    );
    builder.Services.AddSingleton(appConfig);
}

void BuildAppServices(WebApplicationBuilder builder)
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

void BuildSwaggerService(WebApplicationBuilder builder)
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

void BuildAwsServices()
{
    var appConfig = AppConfiguration.Instance;
    var credentials = new BasicAWSCredentials(appConfig.Aws.AwsAccessKeyId, appConfig.Aws.AwsSecretAccessKey);

    var s3Config = new AmazonS3Config()
    {
        RegionEndpoint = RegionEndpoint.SAEast1
    };

    builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(credentials, s3Config));
}