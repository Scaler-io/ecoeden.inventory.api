using Ecoeden.Inventory.Application.Contracts.EventBus;
using Ecoeden.Inventory.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Ecoeden.Inventory.Infrastructure.Factory;
public class PublishServiceFactory(IServiceProvider serviceProvider) : IPublishServiceFactory
{
    public IPublishService<T, TEvent> CreatePublishService<T, TEvent>()
        where T : class
        where TEvent : GenericEvent
    {
        return serviceProvider.GetRequiredService<IPublishService<T, TEvent>>();
    }
}
