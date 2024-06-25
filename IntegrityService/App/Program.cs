using App.Extentions;
using Application;
using DotNetEnv;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ExceptionHandlerMiddleware = Presentation.Middlewares.ExceptionHandlerMiddleware;

//TODO: docker run --name postgres_db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=payze -e POSTGRES_DB=fintech -p 1234:5432 -d postgres
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.ConfigureDiContainer();

builder.Services.ConfigureSwagger();

builder.Services.ConfigureDbContext(builder.Configuration);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ApplicationReferenceClass).Assembly));



var app = builder.Build();

app.EnsureDbCreation();

app.ConfigureSwaggerUI(builder.Configuration);

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapControllers();
app.UseMiddleware<Presentation.Middlewares.ExceptionHandlerMiddleware>();
app.Run();
