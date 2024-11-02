using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Domain.Entities.SQL;
public class EventPublishHistory : SQLBaseEntity
{
    public string EventType { get; set; }
    public string FailureSource { get; set; }
    public string Data { get; set; }
    public EventStatus EventStatus { get; set; }
}
