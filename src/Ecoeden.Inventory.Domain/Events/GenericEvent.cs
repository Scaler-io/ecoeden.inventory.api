using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Domain.Events;
public abstract class GenericEvent
{
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public string CorrelationId { get; set; }
    public Dictionary<string, string> AdditionalProperties { get; set; }
    protected abstract GenericEventType GenericEventType { get; set; }
}
