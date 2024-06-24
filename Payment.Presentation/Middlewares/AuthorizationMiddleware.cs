using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Payment.Application.Command;

namespace Presentation.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly IMediator _mediator;
    public AuthorizationMiddleware(RequestDelegate next, 
        ILogger<ExceptionHandlerMiddleware> logger, 
        IMediator mediator)
    {
        _next = next;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Company-ID", out var companyId) ||
            !context.Request.Headers.TryGetValue("API-Key", out var apiKey) ||
            !context.Request.Headers.TryGetValue("API-Secret", out var apiSecret))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Missing authentication headers");
            return;
        }

        var isValid = await _mediator.Send(
            new AuthenticateCompanyCommand()
            {
                Id = int.Parse(companyId!),
                ApiKey = apiKey!,
                ApiSecret = apiSecret!
            });

        if (!isValid.Response.IsValid)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(isValid.Message);
            return;
        }
        await _next(context);
    }
}