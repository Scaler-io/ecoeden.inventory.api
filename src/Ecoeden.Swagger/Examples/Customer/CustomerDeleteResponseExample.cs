using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Customer;
public class CustomerDeleteResponseExample : IExamplesProvider<bool>
{
    public bool GetExamples()
    {
        return true;    
    }
}
