using Domain.Contracts.Repo.Generic;
using Domain.Models.Entity;

namespace Domain.Contracts.Repo;

public interface ICompanyRepo : IRepo<Company>
{
    ValueTask<Company?> GetCompanyByIdAsync(Guid id);
    Task<int> AddCompanyAsync(Company company);
}