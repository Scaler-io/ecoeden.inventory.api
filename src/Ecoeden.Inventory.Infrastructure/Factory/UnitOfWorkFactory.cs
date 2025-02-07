using Ecoeden.Inventory.Application.Contracts.Database.SQL;
using Ecoeden.Inventory.Application.Contracts.Factory;
using Ecoeden.Inventory.Infrastructure.Database.SQL;
using Microsoft.Extensions.DependencyInjection;

namespace Ecoeden.Inventory.Infrastructure.Factory;
public class UnitOfWorkFactory(IServiceProvider serviceProvider) : IUnitOfWorkFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IUnitOfWork CreateUnitOfWork(string contextName)
    {
        return contextName switch
        {
            "ecoeden" => new UnitOfWork(_serviceProvider.GetRequiredService<EcoedenDbContext>()),
            "ecoedenStock" => new UnitOfWork(_serviceProvider.GetRequiredService<EcoedenStockDbContext>()),
            _ => throw new ArgumentException("Invalid db context name")
        };
    }
}
