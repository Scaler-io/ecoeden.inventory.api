namespace Ecoeden.Inventory.Domain.Models.Core;
public sealed class ApiError
{
    public string Code { get; set; }
    public string Message { get; set; }

    // generic errors

    // contact details errors
    public static ApiError ContactDetailsPhoneInvalidError = new() { Code = "3000", Message = "Given phone number is invalid" };
    public static ApiError ContactDetailsPhoneError = new() { Code = "3001", Message = "Phone number is required" };
    public static ApiError ContactDetailsEmailError = new() { Code = "3002", Message = "Email is invalid" };
    public static ApiError ContactDetialsAddressError = new() { Code = "3003", Message = "Address is required" };

    // contact details address errors
    public static ApiError AddressStreetNameError = new() { Code = "3010", Message = "Street name is required" };
    public static ApiError AddressStreetNumberError = new() { Code = "3011", Message = "Street number is required" };
    public static ApiError AddressCityError = new() { Code = "3010", Message = "City name is required" };
    public static ApiError AddressDistrictError = new() { Code = "3010", Message = "District name is required" };
    public static ApiError AddressStateError = new() { Code = "3010", Message = "State name is required" };
    public static ApiError AddressPostcodeError = new() { Code = "3010", Message = "Postal code is required" };

    // model specific errors
    public static ApiError SupplierNameError = new() { Code = "1001", Message = "Supplier name is required" };
    public static ApiError SupplierContactError = new() { Code = "1002", Message = "Supplier contact details are required" };
    public static ApiError CustomerNameError = new() { Code = "1003", Message = "Customer name is required" };
    public static ApiError CustomerContactError = new() { Code = "1004", Message = "Customer contact details are required" };
    public static ApiError UnitNameError = new() { Code = "1005", Message = "Unit name is required" };

}
