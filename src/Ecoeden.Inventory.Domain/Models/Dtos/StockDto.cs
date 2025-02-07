namespace Ecoeden.Inventory.Domain.Models.Dtos;
public class StockDto
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string SupplierId { get; set; }
    public long Quantity { get; set; }
    public MetadataDto MetaData { get; set; }
}
