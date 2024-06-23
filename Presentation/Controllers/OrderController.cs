using Application.Command;
using Application.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Request;

namespace Presentation.Controllers;

[ApiController]
[Route("api")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/orders")]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        var companyId = Int16.Parse( Request.Headers["Company-ID"]!);
        
        var response = await _mediator.Send(new CreateOrderCommand()
        {
            Amount = request.Amount,
            CompanyId = companyId,
            CreatedAtUtc = DateTime.UtcNow,
            Currency = request.Currency
        });
        
        return Ok(response);
    }

    [HttpGet("/orders/compute")]
    public async Task<IActionResult> ComputeOrders(ComputeOrderRequest request)
    {
        var companyId = Int16.Parse( Request.Headers["Company-ID"]!);

        var response = await _mediator.Send(new ComputeOrdersCommand()
        {
            CompanyId = companyId,
            ComputationId = request.ComputationId
        });

        return Ok(response);
    }

    [HttpGet("orders/computation-status/")]
    public async Task<IActionResult> GetComputationStatus(ComputeOrderRequest request)
    {
        var response = await _mediator.Send(new OrderComputationStatusQuery()
        {
            OrderComputationId = request.ComputationId
        });
        
        return Ok(response);
    }
}