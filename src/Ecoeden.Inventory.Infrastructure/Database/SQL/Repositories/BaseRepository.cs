using Ecoeden.Inventory.Application.Contracts.Database.SQL;
using Ecoeden.Inventory.Domain.Entities.SQL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecoeden.Inventory.Infrastructure.Database.SQL.Repositories;
public class BaseRepository<TEntity>(DbContext context): 
    IBaseRepository<TEntity> where TEntity : SQLBaseEntity
{
    private readonly DbContext _context = context;

    public async Task<TEntity> GetByIdAsync(object id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity> GetEntityByPredicate(Expression<Func<TEntity, bool>> expression)
    {
        return await _context.Set<TEntity>().AsNoTracking().Where(expression).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TEntity>> ListAllAsync()
    {
        return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _context.Set<TEntity>().AsNoTracking().Where(expression).ToListAsync();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _context.Set<TEntity>().AsNoTracking().Where(expression).CountAsync();
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}
