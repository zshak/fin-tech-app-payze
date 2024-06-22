using Application.Query;
using Domain.Contracts.Repo;
using MediatR;

namespace Application.Handlers;

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, GetCompanyQueryResponse?>
{
    private readonly ICompanyRepo _companyRepo;

    public GetCompanyQueryHandler(ICompanyRepo companyRepo)
    {
        _companyRepo = companyRepo;
    }

    public async Task<GetCompanyQueryResponse?> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        var company = await _companyRepo.GetByIdAsync(request.Id);
        return company == null? null :
            new GetCompanyQueryResponse()
            {
                Name = company.Name
            };
    }
}