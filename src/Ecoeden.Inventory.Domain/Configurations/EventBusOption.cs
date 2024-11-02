using System.ComponentModel.DataAnnotations;

namespace Ecoeden.Inventory.Domain.Configurations;
public sealed class EventBusOption
{
    public const string OptionName = "EventBus";
    [Required]
    public string Host { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
