using MongoDB.Driver;

namespace Ecoeden.Inventory.Application.Contracts.Database;
public interface IInventoryDbContext
{
    Task TestDbConnection();
    IMongoDatabase GetDatabaseInstance();
}
