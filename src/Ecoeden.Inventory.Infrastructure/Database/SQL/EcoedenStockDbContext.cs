using Ecoeden.Inventory.Domain.Entities.SQL;
using Ecoeden.Inventory.Infrastructure.Database.SQL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Ecoeden.Inventory.Infrastructure.Database.SQL;
public class EcoedenStockDbContext(DbContextOptions<EcoedenStockDbContext> options): DbContext(options)
{

    public DbSet<ProductStock> ProductStocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductStockEntityConfiguration());
        modelBuilder.HasDefaultSchema("ecoeden.stock");
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<SQLBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdateAt = DateTime.UtcNow;
                    entry.State = EntityState.Modified;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
