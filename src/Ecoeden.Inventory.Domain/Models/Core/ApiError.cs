namespace Ecoeden.Inventory.Domain.Models.Core;
public sealed class ApiError
{
    public string Code { get; set; }
    public string Message { get; set; }

    public ApiError()
    {
        
    }

    public ApiError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    // generic errors

    // contact details errors
    public static ApiError ContactDetailsPhoneInvalidError = new("3000", "Given phone number is invalid");
    public static ApiError ContactDetailsPhoneError = new("3001", "Phone number is required");
    public static ApiError ContactDetailsEmailError = new("3002", "Email is invalid");
    public static ApiError ContactDetialsAddressError = new("3003", "Address is required");

    // contact details address errors
    public static ApiError AddressStreetNameError = new("3010", "Street name is required");
    public static ApiError AddressStreetNumberError = new("3011", "Street number is required");
    public static ApiError AddressCityError = new("3010", "City name is required");
    public static ApiError AddressDistrictError = new("3010", "District name is required");
    public static ApiError AddressStateError = new("3010", "State name is required");
    public static ApiError AddressPostcodeError = new("3010", "Postal code is required");

    // model specific errors
    public static ApiError SupplierNameError = new("1001", "Supplier name is required");
    public static ApiError SupplierContactError = new("1002", "Supplier contact details are required");
    public static ApiError CustomerNameError = new("1003", "Customer name is required");
    public static ApiError CustomerContactError = new("1004", "Customer contact details are required");
    public static ApiError UnitNameError = new("1005", "Unit name is required");
    public static ApiError StockProductError = new("1006", "Product id is required in stock request");
    public static ApiError StockSupplierError = new("1007", "Supplier id is required in stock request");
}
