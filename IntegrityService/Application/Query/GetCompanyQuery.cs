using MediatR;

namespace Application.Query;

public class GetCompanyQuery : IRequest<GetCompanyQueryResponse>
{
    public int Id { get; set; }
}

public class GetCompanyQueryResponse
{
    public required string Name { get; set; }   
}