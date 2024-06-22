using Domain.Contracts.Repo.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Generic;

public class Repo<TEntity> : IRepo<TEntity> where TEntity : class
{
    private readonly CompanyContext _context;
    private readonly DbSet<TEntity> _entities;

    public Repo(CompanyContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public void Update(TEntity entity)
    {
        _entities.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(TEntity entity)
    {
        _entities.Remove(entity);
        _context.SaveChanges();
    }
}