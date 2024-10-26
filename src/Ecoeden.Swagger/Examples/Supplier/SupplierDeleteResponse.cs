using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Supplier;
public class SupplierDeleteResponse : IExamplesProvider<bool>
{
    public bool GetExamples()
    {
        return true;
    }
}
