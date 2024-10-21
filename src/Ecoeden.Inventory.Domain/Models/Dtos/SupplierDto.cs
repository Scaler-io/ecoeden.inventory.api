namespace Ecoeden.Inventory.Domain.Models.Dtos;
public sealed class SupplierDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ContactDetailsDto ContactDetails { get; set; }
    public bool Status { get; set; }
    public MetadataDto Metadata { get; set; }
}
