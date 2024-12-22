namespace Ecoeden.Inventory.Domain.Models.Dtos;
public class UnitDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Status { get; set; }
    public MetadataDto MetaData { get; set; }
}
