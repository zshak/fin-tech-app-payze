using Domain.Models.HttpClient;

namespace Domain.Contracts.HelperService;

public interface IHttpClientService
{
    public Task<bool> IsAuthenticCompany(CompanyAuthenticationMetadata companyAuthenticationMetadata);
}