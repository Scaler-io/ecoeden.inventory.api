using System.ComponentModel.DataAnnotations;

namespace Ecoeden.Inventory.Domain.Configurations;
public sealed class MongoDbOption
{
    public const string OptionName = "MongoDb";
    [Required]
    public string ConnectionString { get; set; }
    public string Database { get; set; }
}
