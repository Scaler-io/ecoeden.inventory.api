using Ecoeden.Inventory.Domain.Events;
using Ecoeden.Inventory.Domain.Models.Enums;

namespace Contracts.Events;
public class ProductStockUpdated : GenericEvent
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string SupplierId { get; set; }
    public long Quantity { get; set; }
    protected override GenericEventType GenericEventType { get; set; } = GenericEventType.StockUpdated;
}
