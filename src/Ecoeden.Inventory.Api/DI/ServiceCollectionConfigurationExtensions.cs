using Ecoeden.Inventory.Domain.Configurations;

namespace Ecoeden.Inventory.Api.DI;

public static class ServiceCollectionConfigurationExtensions
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<LoggingOption>()
            .BindConfiguration(LoggingOption.OptionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddOptions<AppConfigOption>()
            .BindConfiguration(AppConfigOption.OptionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddOptions<ElasticSearchOption>()
            .BindConfiguration(ElasticSearchOption.OptionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddOptions<IdentityGroupAccessOption>()
            .BindConfiguration(IdentityGroupAccessOption.OptionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<MongoDbOption>()
            .BindConfiguration(MongoDbOption.OptionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<EventBusOption>()
            .BindConfiguration(EventBusOption.OptionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
