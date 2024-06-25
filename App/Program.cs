using App.Extentions;
using Application;
using Infrastructure.HostedServices;
using Infrastructure.Implementations.HelperService.Rabbitmq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.ConfigureDiContainer();

builder.Services.ConfigureSwagger();

builder.Services.ConfigureDbContext(builder.Configuration);

builder.Services.ConfigureConfig(builder.Configuration);

builder.Services.AddHostedService<OrderPublisherJob>();
builder.Services.AddHostedService<OrderComputationRabbitmqConsumer>();
builder.Services.AddHostedService<PaymentStatusRabbitmqConsumer>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ApplicationReferenceClass).Assembly));


var app = builder.Build();

app.EnsureDbCreation();

app.ConfigureSwaggerUI(builder.Configuration);

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapControllers();
app.UseMiddleware<Presentation.Middlewares.ExceptionHandlerMiddleware>();
app.UseMiddleware<Presentation.Middlewares.AuthorizationMiddleware>();
app.Run();