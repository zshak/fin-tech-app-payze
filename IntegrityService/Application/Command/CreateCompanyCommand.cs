using Application.Command.CommandResponse;
using Domain.Models.Entity;
using MediatR;

namespace Application.Command;

public class CreateCompanyCommand : IRequest<CommandResponse<CreateCompanyCommandResponse>>
{
    public string Name { get; set; }
}

public class CreateCompanyCommandResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public Guid ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}