namespace Ecoeden.Inventory.Domain.Entities.SQL;
public class SQLBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    public string CorrelationId { get; set; }
}
