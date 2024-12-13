using Ecoeden.Inventory.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Customer;
public class CustomerDetailsResponseExample : IExamplesProvider<CustomerDto>
{
    public CustomerDto GetExamples()
    {
        return new()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Person 1",
            Status = true,
            ContactDetails = new()
            {
                Email = "test@email.com",
                Phone = "+917554599987",
                Address = new()
                {
                    StreetNumber = "11",
                    StreetName = "Pati colony",
                    StreetType = "Road",
                    City = "Siliguri",
                    District = "Darjeeling",
                    State = "West bengal",
                    PostCode = "734003"
                }
            }
        };
    }
}
