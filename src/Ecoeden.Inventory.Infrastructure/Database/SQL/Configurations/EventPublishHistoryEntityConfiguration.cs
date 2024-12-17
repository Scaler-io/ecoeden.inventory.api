using Ecoeden.Inventory.Domain.Entities.SQL;
using Ecoeden.Inventory.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecoeden.Inventory.Infrastructure.Database.SQL.Configurations;
public class EventPublishHistoryEntityConfiguration : IEntityTypeConfiguration<EventPublishHistory>
{
    public void Configure(EntityTypeBuilder<EventPublishHistory> builder)
    {
        builder.ToTable("EventPublishHistories", "ecoeden.event");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.EventType).IsRequired().HasMaxLength(25);
        builder.Property(c => c.Data).IsRequired();
        builder.Property(c => c.FailureSource).IsRequired().HasMaxLength(25);
        builder.Property(c => c.EventStatus)
            .IsRequired()
            .HasConversion(o => o.ToString(), o => (EventStatus)Enum.Parse(typeof(EventStatus), o))
            .HasMaxLength(25);
    }
}
