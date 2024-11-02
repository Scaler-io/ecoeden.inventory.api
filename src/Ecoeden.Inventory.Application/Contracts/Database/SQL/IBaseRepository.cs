using Ecoeden.Inventory.Domain.Entities.SQL;

namespace Ecoeden.Inventory.Application.Contracts.Database.SQL;
public interface IBaseRepository<TEntity> where TEntity : SQLBaseEntity
{
    Task<TEntity> GetByIdAsync(object id);
    Task<IReadOnlyList<TEntity>> ListAllAsync();
    Task<TEntity> GetEntityWithSpec(ISpecification<TEntity> spec);
    Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec);
    Task<int> CountAsync(ISpecification<TEntity> spec);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
