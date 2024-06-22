using Application.Command;
using Application.Command.CommandResponse;
using Domain.Contracts.Repo;
using Domain.Contracts.Strategy;
using MediatR;

namespace Application.Handlers;

public class
    ValidateCompanyCommandHandler : IRequestHandler<ValidateCompanyCommand, CommandResponse<ValidateCompanyCommandResponse>>
{
    private readonly ICompanyRepo _companyRepo;
    private readonly IApiKeyStrategy _apiKeyStrategy;
    public ValidateCompanyCommandHandler(
        ICompanyRepo companyRepo, 
        IApiKeyStrategy apiKeyStrategy)
    {
        _companyRepo = companyRepo;
        _apiKeyStrategy = apiKeyStrategy;
    }

    public async Task<CommandResponse<ValidateCompanyCommandResponse>> Handle(ValidateCompanyCommand request, CancellationToken cancellationToken)
    {
        var b = Guid.TryParse(request.ApiKey, out _);
        if (request.ApiKey == "" || !Guid.TryParse(request.ApiKey,out _) || request.ApiSecret == "")
        {
            return new CommandResponse<ValidateCompanyCommandResponse>()
            {
                Success = true,
                Message = "Api Key Or Secret Not Provided",
                Response = new ValidateCompanyCommandResponse()
                {
                    IsValid = false
                }
            };
        }
        
        var company = await _companyRepo.GetByIdAsync(request.Id);

        if (company == null)
        {
            return new CommandResponse<ValidateCompanyCommandResponse>()
            {
                Success = true,
                Message = "Company Not Found",
                Response = new ValidateCompanyCommandResponse()
                {
                    IsValid = false
                }
            };
        }
        var isValid = _apiKeyStrategy.IsValidApiKey(company.ApiKey, Guid.Parse(request.ApiKey)) &&
                      _apiKeyStrategy.IsValidApiSecret(company.ApiSecret, request.ApiSecret);

        if (isValid)
        {
            return new CommandResponse<ValidateCompanyCommandResponse>()
            {
                Success = true,
                Message = "Valid Company",
                Response = new ValidateCompanyCommandResponse()
                {
                    IsValid = true
                }
            };
        }
        
        return new CommandResponse<ValidateCompanyCommandResponse>()
        {
            Success = true,
            Message = "Invalid Company",
            Response = new ValidateCompanyCommandResponse()
            {
                IsValid = false
            }
        };
    }
}