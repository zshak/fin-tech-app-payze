using MediatR;
using Payment.Application.Command;
using Payment.Application.CQRSbase;
using Payment.Domain.Contracts.HelperService;
using Payment.Domain.Models.HttpClient;

namespace Payment.Application.Handlers;

public class AuthenticateCompanyCommandHandler : IRequestHandler<
    AuthenticateCompanyCommand, BaseResponse<AuthenticateCompanyCommandResponse>>
{
    private readonly IHttpClientService _httpClientService;

    public AuthenticateCompanyCommandHandler(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public async Task<BaseResponse<AuthenticateCompanyCommandResponse>> Handle(AuthenticateCompanyCommand request, CancellationToken cancellationToken)
    {
        var isValidCompany = await _httpClientService.IsAuthenticCompany(new CompanyAuthenticationMetadata()
        {
            Id = request.Id,
            ApiKey = request.ApiKey,
            ApiSecret = request.ApiSecret
        });

        return new BaseResponse<AuthenticateCompanyCommandResponse>()
        {
            Success = true,
            Message = isValidCompany? "Valid Company" : "InvalidCompany",
            Response = new AuthenticateCompanyCommandResponse()
            {
                IsValid = isValidCompany
            }
        };
    }
}