using Application.Implementations.HelperService;
using Domain.Contracts.HelperService;
using Microsoft.EntityFrameworkCore;
using Payment.Application.Implementations.Factory;
using Payment.Domain.Factory;
using Payment.Infrastructure.Implementations.HelperService.Rabbitmq;

namespace Payment.App.Extentions;

using Domain.Config;
using Domain.Contracts.HelperService;
using Domain.Contracts.Repo;
using Infrastructure;
using Infrastructure.Implementations.Repo;
using Infrastructure.Implementations.Repo.Generic;


public static class ServiceExtentions
{
    public static IServiceCollection ConfigureDiContainer(this IServiceCollection services)
    {
        // Add repositories
        services.AddScoped<IOrderPaymentRepo, OrderPaymentRepo>();
        
        services.AddSingleton<IRabbitMqPublisherService, RabbitMqPublisherService>();
        
        services.AddSingleton<IHttpClientService, HttpClientService>();

        services.AddSingleton<IPaymentServiceSenderStrategyFactory, PaymentServiceSenderStrategyFactory>();
        return services;
    }
    
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<OrderPaymentRabbitmqConsumer>();
        return services;
    }
    
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<PaymentContext>();

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
    
    public static WebApplication EnsureDbCreation(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {   
            var dbContext = services.GetRequiredService<PaymentContext>();
            dbContext.Database.Migrate();
        } 
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        return app;
    }
}