using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;

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
using PersonalTasksProject.Extensions;
using PersonalTasksProject.Middlewares;
using PersonalTasksProject.Providers;
using PersonalTasksProject.Providers.Interfaces;
using PersonalTasksProject.Repositories.Implementations;
using PersonalTasksProject.Repositories.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

AppConfiguration.BuildAppLogger(builder);
AppConfiguration.BuildAppConfigurations(builder);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();

AppConfiguration.BuildSwaggerService(builder);

AppConfiguration.BuildAppServices(builder);

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

AppConfiguration.BuildAwsServices(builder);

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
