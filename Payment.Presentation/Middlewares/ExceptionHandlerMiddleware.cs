using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Payment.App.Exceptions.Base;

namespace Presentation.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseException myException)
        {
            _logger.LogError($@"Custom Exception Occured: Message -> {myException.Message}; 
                                Stack trace -> {myException.StackTrace}");
            context.Response.StatusCode = myException.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync($"{myException.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error: {ex}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
        }
    }
}