using Ecoeden.Inventory.Application.Contracts.Database.SQL;

namespace Ecoeden.Inventory.Application.Contracts.Factory;
public interface IUnitOfWorkFactory
{
    IUnitOfWork CreateUnitOfWork(string contextName);
}
