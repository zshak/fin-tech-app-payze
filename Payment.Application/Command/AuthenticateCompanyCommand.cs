using MediatR;
using Payment.Application.CQRSbase;

namespace Payment.Application.Command;

public class AuthenticateCompanyCommand : IRequest<BaseResponse<AuthenticateCompanyCommandResponse>>
{
    public int Id { get; set; }
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}


public class AuthenticateCompanyCommandResponse 
{
    public required bool IsValid { get; set; }
}