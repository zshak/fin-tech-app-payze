using Application.Command;
using Application.Query;
using MediatR;
using Presentation.DTOs.Request;

namespace Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class IntegrityController : ControllerBase
{
    private readonly IMediator _mediator;

    public IntegrityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/companies")]
    public async Task<IActionResult> CreateCompany(CreateCompanyRequest request)
    {
        var response = await _mediator.Send(new CreateCompanyCommand()
        {
            Name = request.CompanyName
        });
        return Ok(response);
    }

    [HttpGet("/companies/{id}")]
    public async Task<IActionResult> GetCompany(int id)
    {
        var response = await _mediator.Send(new GetCompanyQuery()
        {
            Id = id
        });
        return Ok(response);
    }

    [HttpPost("/companies/{id}/validate")]
    public async Task<IActionResult> ValidateCompany(int id, ValidateCompanyRequest request)
    {
        var apiKey = request.ApiKey;
        var apiSecret = request.ApiSecret;
        var response = await _mediator.Send(new ValidateCompanyCommand()
        {
            Id = id,
            ApiKey = apiKey,
            ApiSecret = apiSecret
        });

        return Ok(response);
    }
}