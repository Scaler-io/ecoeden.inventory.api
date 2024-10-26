namespace Ecoeden.Inventory.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ValidateNestedAttribute : Attribute
{
}
