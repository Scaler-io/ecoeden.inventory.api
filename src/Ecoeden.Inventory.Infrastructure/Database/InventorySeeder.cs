using Ecoeden.Inventory.Application.Contracts.Database;
using Ecoeden.Inventory.Application.Helpers;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Constants;
using MongoDB.Driver;

namespace Ecoeden.Inventory.Infrastructure.Database;
public static class InventorySeeder
{
    public static async Task SeedAsync(IInventoryDbContext context)
    {
        IMongoDatabase mongoDatabase = context.GetDatabaseInstance();

        var supplierCollection = mongoDatabase.GetCollection<Supplier>(MongoDbCollectionNames.Suppliers);

        if (await supplierCollection.CountDocumentsAsync(_ => true) < 1)
        {
            var suppliers = FileReaderHelper<Supplier>.ReadFile("suppliers", "./AppData");
            await supplierCollection.InsertManyAsync(suppliers);
        }
    }
}
