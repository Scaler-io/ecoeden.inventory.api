using Ecoeden.Inventory.Domain.Events;

namespace Ecoeden.Inventory.Application.Contracts.EventBus;
public interface IPublishService<T, TEvent>
    where T : class
    where TEvent : GenericEvent
{
    Task PublishAsync(T message, string correlationId, Dictionary<string, string> additionalProperties = null);
}
