using Application.Command;
using Application.Command.CommandResponse;
using Domain.Contracts.Repo;
using Domain.Contracts.Strategy;
using Domain.Models.Entity;
using MediatR;

namespace Application.Handlers;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CommandResponse<CreateCompanyCommandResponse>>
{
    private readonly ICompanyRepo _companyRepo;
    private readonly IApiKeyStrategy _apiKeyStrategy;
    
    public CreateCompanyCommandHandler(
        ICompanyRepo companyRepo, 
        IApiKeyStrategy apiKeyStrategy)
    {
        _companyRepo = companyRepo;
        _apiKeyStrategy = apiKeyStrategy;
    }

    public async Task<CommandResponse<CreateCompanyCommandResponse>> Handle(
        CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        Guid apiKey = _apiKeyStrategy.GenerateApiKey();
        string apiSecret = _apiKeyStrategy.GenerateApiSecret();
        
        int companyId = await _companyRepo.AddCompanyAsync(new Company()
        {
            Name = request.Name,
            ApiKey = apiKey,
            ApiSecret = apiSecret
        });

        return new CommandResponse<CreateCompanyCommandResponse>()
        {
            Success = true,
            Message = "CompanyCreatedSuccessfully",
            Response = new CreateCompanyCommandResponse()
            {
                Id = companyId,
                Name = request.Name,
                ApiSecret = apiSecret,
                ApiKey = apiKey
            }
        };
    }
}