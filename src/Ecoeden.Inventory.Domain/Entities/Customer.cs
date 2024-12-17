namespace Ecoeden.Inventory.Domain.Entities;
public class Customer : BaseEntity
{
    public string Name { get; set; }
    public bool Status { get; set; }
    public ContactDetails ContactDetails { get; set; }
}
