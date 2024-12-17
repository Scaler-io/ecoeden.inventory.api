namespace Ecoeden.Inventory.Domain.Configurations;
public sealed class AppConfigOption
{
    public const string OptionName = "AppConfigurations";
    public string ApplicationIdentifier { get; set; }
    public string ApplicationEnvironment { get; set; }
    public int HealthCheckTimeoutInSeconds { get; set; }
    public int CacheExpiration { get; set; }
    public string SupplierStorageCacheKey { get; set; }
    public string CustomerStorageCacheKey { get; set; }
}
