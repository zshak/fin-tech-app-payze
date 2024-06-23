using Application.Implementations.HelperService;
using Domain.Config;
using Domain.Contracts.HelperService;
using Domain.Contracts.Repo;
using Infrastructure;
using Infrastructure.Implementations.Repo;
using Infrastructure.Implementations.Repo.Generic;

namespace App.Extentions;

public static class ServiceExtentions
{
    public static IServiceCollection ConfigureDiContainer(this IServiceCollection services)
    {
        // Add repositories
        services.AddSingleton<IHttpClientService, HttpClientService>();

        services.AddScoped<IOrderRepo, OrderRepo>();
        services.AddScoped<IOrderComputationStatusRepo, OrderComputationStatusRepo>();

        services.AddSingleton<IRabbitMqPublisherService, RabbitMqPublisherService>();
        return services;
    }
    
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<OrderContext>();

        return services;
    }
    
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        
        return services;
    }
    
    public static IServiceCollection ConfigureConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServiceUrls>(configuration.GetSection("ServiceUrls"));
        services.Configure<RabbitMqConfig>(configuration.GetSection("RabbitMq"));
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