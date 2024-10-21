namespace Ecoeden.Inventory.Domain.Models.Core;
public sealed class ApiError
{
    public string Code { get; set; }
    public string Message { get; set; }

    // generic errors
    public static ApiError InvalidPhoneNumberError = new() { Code = "3000", Message = "Given phone number is invalid" };

    // model specific errors
    public static ApiError SupplierNameError = new() { Code = "1001", Message = "Supplier name is required" };
    public static ApiError SupplierPhoneError = new() { Code = "1002", Message = "Supplier phone is required" };
    public static ApiError SupplierAddressError = new() { Code = "1003", Message = "Supplier address is required" };
}
