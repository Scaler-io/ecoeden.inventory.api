namespace Ecoeden.Inventory.Domain.Entities;
public class Supplier(string name, string phone, string address, bool status) : BaseEntity
{
    public string Name { get; set; } = name;
    public string Phone { get; set; } = phone;
    public string Address { get; set; } = address;
    public bool Status { get; set; } = status;
}
