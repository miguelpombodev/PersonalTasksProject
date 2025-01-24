using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PersonalTasksProject.Business.Implementations;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.Context;
using PersonalTasksProject.DTOs.Mappings;
using PersonalTasksProject.Middlewares;
using PersonalTasksProject.Providers;
using PersonalTasksProject.Repositories.Implementations;
using PersonalTasksProject.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
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


var connectionString = builder.Configuration.GetConnectionString("MainDatabaseCS");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(
        connectionString
        )
    );

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(CreateUserDtoMappingProfile));
builder.Services.AddAutoMapper(typeof(CreateTaskDtoMappingProfile));

builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddSingleton<SmtpEmailProvider>();
builder.Services.AddSingleton<FileProvider>();

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
            option.SaveToken = true;
            option.RequireHttpsMetadata = false;
            option.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        }
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapControllers();
app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();


app.Run();