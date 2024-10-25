using Ecoeden.Inventory.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Supplier;
public class SupplierListResponseExample : IExamplesProvider<List<SupplierDto>>
{
    public List<SupplierDto> GetExamples()
    {
        return [new() {
           Id = Guid.NewGuid().ToString(),
            Name = "Test supplier",
            Status = true,
            ContactDetails = new()
            {
                Email = "testsupplier@email.com",
                Phone = "+910123456789",
                Address = new()
                {
                    StreetNumber = "12",
                    StreetName = "street name",
                    StreetType = "Road",
                    City = "city",
                    District = "District",
                    PostCode = "700000",
                    State = "state"
                }
            },
            Metadata = new()
            {
                CreatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                CreatedBy = "Deafult",
                UpdatedBy = "Deafult"
            }
       }];
    }
}
