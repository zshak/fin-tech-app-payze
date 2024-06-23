using Application.CQRSbase;
using MediatR;

namespace Application.Command;

public class ComputeOrdersCommand : IRequest<BaseResponse<ComputeOrdersCommandResponse>>
{
    public Guid ComputationId { get; set; }
    public int CompanyId { get; set; }
}

public class ComputeOrdersCommandResponse
{
    
}