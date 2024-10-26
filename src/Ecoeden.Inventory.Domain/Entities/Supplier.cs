namespace Ecoeden.Inventory.Domain.Entities;
public sealed class Supplier : BaseEntity
{
    public string Name { get; set; }
    public ContactDetails ContactDetails { get; set; }
    public bool Status { get; set; } = false;

}
