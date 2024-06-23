using Application.Models;
using Application.Models.IdentityService;
using Domain.Config;
using Domain.Contracts.HelperService;
using Domain.Exceptions;
using Domain.Models.HttpClient;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Application.Implementations.HelperService;

public class HttpClientService : IHttpClientService
{
    private readonly ServiceUrls _serviceUrls;

    public HttpClientService(IOptions<ServiceUrls> serviceUrls)
    {
        _serviceUrls = serviceUrls.Value;
    }

    public async Task<bool> IsAuthenticCompany(CompanyAuthenticationMetadata companyAuthenticationMetadata)
    {
        using var restClient = new RestClient(_serviceUrls.IntegrityService);

        var request = new RestRequest($"companies/{companyAuthenticationMetadata.Id}/validate", Method.Post);

        request.AddJsonBody(new IdentityServiceAuthenticationModel()
        {
            ApiKey = companyAuthenticationMetadata.ApiKey,
            ApiSecret = companyAuthenticationMetadata.ApiSecret
        });

        var response =
            await restClient.ExecuteAsync<IdentityServiceBaseResponse<IdentityServiceAuthenticationResponse>>(
                request);

        if (!response.IsSuccessful || response.Data.Success == false)
        {
            throw new AuthenticationUnavailableException();
        }

        return response.Data.Response.IsValid;
    }
}