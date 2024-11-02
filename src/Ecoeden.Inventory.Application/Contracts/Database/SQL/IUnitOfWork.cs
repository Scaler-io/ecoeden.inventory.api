using Ecoeden.Inventory.Domain.Entities.SQL;

namespace Ecoeden.Inventory.Application.Contracts.Database.SQL;
public interface IUnitOfWork
{
    IBaseRepository<TEntity> Repository<TEntity>() where TEntity : SQLBaseEntity;
    Task<int> Complete();
}
