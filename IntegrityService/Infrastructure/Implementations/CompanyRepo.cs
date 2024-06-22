using Domain.Contracts.Repo;
using Domain.Contracts.Repo.Generic;
using Domain.Models.Entity;
using Infrastructure.Implementations.Generic;

namespace Infrastructure.Implementations;

public class CompanyRepo : Repo<Company>,ICompanyRepo
{
    private readonly CompanyContext _companyContext;

    public CompanyRepo(CompanyContext companyContext) : base(companyContext)
    {
        _companyContext = companyContext;
    }

    public ValueTask<Company?> GetCompanyByIdAsync(Guid id)
    {
        return _companyContext.Companies.FindAsync(id);
    }

    public async Task<int> AddCompanyAsync(Company company)
    {
        _companyContext.Companies.Add(company);
        await _companyContext.SaveChangesAsync();
        return company.Id;
    }
}