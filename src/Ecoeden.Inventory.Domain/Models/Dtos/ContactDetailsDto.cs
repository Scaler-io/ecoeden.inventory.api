namespace Ecoeden.Inventory.Domain.Models.Dtos;
public sealed class ContactDetailsDto
{
    public string Email { get; set; }
    public string Phone { get; set; }
    public AddressDto Address { get; set; }
}
