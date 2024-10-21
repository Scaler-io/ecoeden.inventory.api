namespace Ecoeden.Inventory.Domain.Entities;
public sealed class Address
{
    public string StreetNumber { get; set; }
    public string StreetName { get; set; }
    public string StreetType { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string State { get; set; }
    public string PostCode { get; set; }
}
