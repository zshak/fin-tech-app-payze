using Microsoft.EntityFrameworkCore;
using Payment.Domain.Contracts.Repo.Generic;

namespace Payment.Infrastructure.Implementations.Repo.Generic;

public class Repo<TEntity> : IRepo<TEntity> where TEntity : class
{
    private readonly PaymentContext _context;
    private readonly DbSet<TEntity> _entities;

    public Repo(PaymentContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
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

    public async Task UpdateAsync(TEntity entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _entities.Remove(entity);
        await _context.SaveChangesAsync();
    }
}