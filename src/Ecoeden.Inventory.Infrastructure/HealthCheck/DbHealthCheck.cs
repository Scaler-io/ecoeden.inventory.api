using Ecoeden.Inventory.Application.Contracts.Database;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ecoeden.Inventory.Infrastructure.HealthCheck;
public sealed class DbHealthCheck(IInventoryDbContext dbContext) : IHealthCheck
{
    private readonly IInventoryDbContext _dbContext = dbContext;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.TestDbConnection();
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
