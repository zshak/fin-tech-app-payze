using Application.Command;
using Application.CQRSbase;
using Domain.Contracts.HelperService;
using Domain.Models.HttpClient;
using MediatR;

namespace Application.Handlers.Command;

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