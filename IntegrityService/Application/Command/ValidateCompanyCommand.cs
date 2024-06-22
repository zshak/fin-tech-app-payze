using MediatR;
using Application.Command.CommandResponse;
namespace Application.Command;

public class ValidateCompanyCommand : IRequest<CommandResponse<ValidateCompanyCommandResponse>>
{
    public int Id { get; set; }
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}

public class ValidateCompanyCommandResponse
{
    public required bool IsValid { get; set; }
}