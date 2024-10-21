namespace Ecoeden.Inventory.Application.Validators;
public class PhoneValidator
{
    public static bool IsValid(string phone)
    {
        return phone.StartsWith("+91") && phone.Substring(1).Length == 10;
    }
}
