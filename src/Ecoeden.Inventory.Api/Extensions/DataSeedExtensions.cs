using Ecoeden.Inventory.Application.Contracts.Database;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Api.Extensions;

public static class DataSeedExtensions
{
    public static WebApplication SeedMongoDatabase(this WebApplication app,
        Action<IInventoryDbContext> seeder, int? retry = 0)
    {
        int retryForAvailability = retry.Value;
        using var scope = app.Services.CreateScope();

        var serviceProviders = scope.ServiceProvider;
        var logger = serviceProviders.GetRequiredService<ILogger>();
        var databaseContext = serviceProviders.GetRequiredService<IInventoryDbContext>();

        try
        {
            logger.Here().Information("Migrating database with context {@dbContext}", typeof(IInventoryDbContext).Name);

            seeder(databaseContext);

            logger.Here().Information("Migrated database with context {@dbContext}", typeof(IInventoryDbContext).Name);
        }
        catch (Exception ex)
        {
            logger.Here().Error("{@ErrorCode} Migration failed. {@Message} - {@StackTrace}", ErrorCodes.OperationFailed, ex.Message, ex.StackTrace);
            if (retryForAvailability < 5)
            {
                retryForAvailability++;
                Thread.Sleep(2000);
                SeedMongoDatabase(app, seeder, retryForAvailability);
            }
        }

        return app;
    }
}
