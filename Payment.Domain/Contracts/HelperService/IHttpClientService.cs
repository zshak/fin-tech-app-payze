using Payment.Domain.Models.HttpClient;

namespace Payment.Domain.Contracts.HelperService;

public interface IHttpClientService
{
    public Task<bool> IsAuthenticCompany(CompanyAuthenticationMetadata companyAuthenticationMetadata);
}