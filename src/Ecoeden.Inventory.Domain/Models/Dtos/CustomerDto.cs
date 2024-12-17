namespace Ecoeden.Inventory.Domain.Models.Dtos;
public class CustomerDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Status { get; set; }
    public ContactDetailsDto ContactDetails { get; set; }
    public MetadataDto MetaData { get; set; }
}
