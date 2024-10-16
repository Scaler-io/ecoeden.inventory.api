using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Domain.Models.Core;
public sealed class RequestInformation
{
    public UserDto CurrentUser { get; set; }
    public string CorrelationId { get; set; }
}
