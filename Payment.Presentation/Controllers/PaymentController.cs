using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Command;
using Payment.Domain.Exceptions;
using Payment.Presentation.DTOs;
using Payment.Presentation.StaticHelpers;
using Payment.Presentation.Validator;

namespace Payment.Presentation.Controllers;


[ApiController]
[Route("api")]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly PayForOrderValidator _validator;
    public PaymentController(
        IMediator mediator,
        PayForOrderValidator validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpPost("/orders")]
    public async Task<IActionResult> CreateOrder(PayRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new InvalidCardNumberException(MessageConverter.ConvertValidationFailureListToMessage(validationResult.Errors));
        
        var companyId = Int16.Parse( Request.Headers["Company-ID"]!);
        
        var response = await _mediator.Send(new PayForOrderCommand()
        {
            CompanyId = companyId,
            CardNumber = request.CardNumber,
            OrderId = request.OrderId,
            ExpiryDate = request.ExpiryDate
        });

        return Ok(response);
    }
}