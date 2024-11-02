﻿using Ecoeden.Inventory.Domain.Entities.SQL;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ecoeden.Inventory.Infrastructure.Database.SQL;
public class EcoedenDbContext(DbContextOptions<EcoedenDbContext> options) : DbContext(options)
{
    public DbSet<EventPublishHistory> EventPublishHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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