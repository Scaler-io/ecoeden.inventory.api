using Ecoeden.Inventory.Domain.Entities.SQL;
using System.Linq.Expressions;

namespace Ecoeden.Inventory.Application.Contracts.Database.SQL;
public interface IBaseRepository<TEntity> where TEntity : SQLBaseEntity
{
    Task<TEntity> GetByIdAsync(object id);
    Task<IReadOnlyList<TEntity>> ListAllAsync();
    Task<TEntity> GetEntityByPredicate(Expression<Func<TEntity, bool>> expression);
    Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
