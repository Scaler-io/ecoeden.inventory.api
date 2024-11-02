using Ecoeden.Inventory.Domain.Events;
using Ecoeden.Inventory.Domain.Models.Enums;

namespace Contracts.Events;
public class SupplierDeleted : GenericEvent
{
    public string Id { get; set; }
    protected override GenericEventType GenericEventType { get; set; } = GenericEventType.SupplierDeleted;
}
