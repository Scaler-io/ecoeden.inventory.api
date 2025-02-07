using Ecoeden.Inventory.Application.Contracts.Database.SQL;
using Ecoeden.Inventory.Domain.Entities.SQL;
using Ecoeden.Inventory.Infrastructure.Database.SQL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Ecoeden.Inventory.Infrastructure.Database.SQL;
public class UnitOfWork(DbContext context) : IUnitOfWork
{
    private readonly DbContext _context = context;
    private Hashtable _repositories;

    public async Task<int> Complete() => await _context.SaveChangesAsync();

    public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : SQLBaseEntity
    {
        _repositories ??= [];
        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(BaseRepository<>)  ;
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
            _repositories.Add(type, repositoryInstance);
        }

        return (IBaseRepository<TEntity>)_repositories[type];
    }
}
