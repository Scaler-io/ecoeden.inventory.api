using Microsoft.Extensions.DependencyInjection;

namespace Ecoeden.Inventory.Application.DI;
public static class BusinessLogicServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services)
    {
        return services;
    }
}
