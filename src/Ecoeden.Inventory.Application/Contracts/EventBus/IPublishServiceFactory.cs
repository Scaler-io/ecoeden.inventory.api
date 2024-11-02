using Ecoeden.Inventory.Domain.Events;

namespace Ecoeden.Inventory.Application.Contracts.EventBus;
public interface IPublishServiceFactory
{
    IPublishService<T, TEvent> CreatePublishService<T, TEvent>()
        where T : class
        where TEvent : GenericEvent;
}
