using Application.Implementations.Strategy;
using Domain.Contracts.Repo;
using Domain.Contracts.Strategy;
using Infrastructure;
using Infrastructure.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace App.Extentions;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection ConfigureDiContainer(this IServiceCollection services)
    {
        // Add repositories
        services.AddScoped<ICompanyRepo, CompanyRepo>();
        
        //strategy
        services.AddScoped<IApiKeyStrategy, ApiKeyDefaultStrategy>();
        return services;
    }
    
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<CompanyContext>();

        return services;
    }
    
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        
        return services;
    }
    
    public static ConfigurationManager ConfigureConnectionString(this ConfigurationManager configuration)
    {
        // Build the connection string from environment variables
        var host = Environment.GetEnvironmentVariable("DB_HOST");
        var database = Environment.GetEnvironmentVariable("DB_NAME");
        var username = Environment.GetEnvironmentVariable("DB_USERNAME");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var port = Environment.GetEnvironmentVariable("DB_PORT");

        var connectionString = $"Host={host};Database={database};Username={username};Password={password};Port={port}";
        configuration["ConnectionStrings:DefaultConnection"] = connectionString;

        return configuration;
    }
    
    // ReSharper disable once InconsistentNaming
    public static WebApplication ConfigureSwaggerUI(this WebApplication app, IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        
        return app;
    }
}