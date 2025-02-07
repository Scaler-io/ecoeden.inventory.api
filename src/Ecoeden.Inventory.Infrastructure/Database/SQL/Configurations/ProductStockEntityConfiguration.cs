using Ecoeden.Inventory.Domain.Entities.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecoeden.Inventory.Infrastructure.Database.SQL.Configurations;
public class ProductStockEntityConfiguration : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(EntityTypeBuilder<ProductStock> builder)
    {
        builder.ToTable("ProductStocks", "ecoeden.stock");
        builder.Property(c => c.Id).ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");

        builder.Property(p => p.ProductId).IsRequired();
        builder.Property(p => p.SupplierId).IsRequired();
        builder.Property(p => p.Quantity).IsRequired();

        builder.HasKey(p => new { p.ProductId, p.SupplierId}).IsClustered();
    }
}
