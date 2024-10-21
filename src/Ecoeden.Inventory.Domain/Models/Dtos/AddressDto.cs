namespace Ecoeden.Inventory.Domain.Models.Dtos;
public sealed class AddressDto
{
    public string StreetNumber { get; set; }
    public string StreetName { get; set; }
    public string StreetType { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string State { get; set; }
    public string PostCode { get; set; }
}
