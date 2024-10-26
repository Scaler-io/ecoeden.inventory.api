namespace Ecoeden.Inventory.Application.Validators;
public class PhoneValidator
{
    public static bool IsValid(string phone)
    {
        if(string.IsNullOrEmpty(phone)) return false;
        return phone.StartsWith("+91") && phone.Substring(3).Length == 10;
    }
}
