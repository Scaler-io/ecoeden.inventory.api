namespace Ecoeden.Inventory.Domain.Entities.SQL;
public class ProductStock : SQLBaseEntity
{
    public string ProductId { get; set; }
    public string SupplierId { get; set; }
    public long Quantity { get; set; } = 0;
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }

    public void UpdateCreationData(string userId, string correlationId)
    {
        CreatedBy = userId;
        CorrelationId = correlationId;
    }

    public void UpdateUpdationData(string userId, string correlationId)
    {
        UpdatedBy = userId;
        CorrelationId = correlationId;
    } 
}
