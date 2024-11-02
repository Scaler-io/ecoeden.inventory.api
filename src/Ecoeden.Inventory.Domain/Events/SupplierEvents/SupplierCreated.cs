using Ecoeden.Inventory.Domain.Events;
using Ecoeden.Inventory.Domain.Models.Enums;

namespace Contracts.Events;
public class SupplierCreated : GenericEvent
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public bool Status { get; set; }
    protected override GenericEventType GenericEventType { get; set; } = GenericEventType.SupplierCreated;
}
