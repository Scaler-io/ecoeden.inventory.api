using Ecoeden.Inventory.Application.Contracts.Database;
using Ecoeden.Inventory.Domain.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ecoeden.Inventory.Infrastructure.Database;
public sealed class InventoryDbContext(IOptions<MongoDbOption> mongodbOptions, MongoClient client) : IInventoryDbContext
{
    private readonly IMongoDatabase _mongoDatabase = client.GetDatabase(mongodbOptions.Value.Database);

    public IMongoDatabase GetDatabaseInstance()
    {
        return _mongoDatabase;
    }

    public async Task TestDbConnection()
    {
        await _mongoDatabase.ListCollectionNamesAsync();
    }
}
