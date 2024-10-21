using Microsoft.Extensions.DependencyInjection;

namespace Ecoeden.Inventory.Application.DI;
public static class RegisteredValdatorContext
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {

        return services;
    }
}
