using Ecoeden.Inventory.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Unit;
public class UnitResponseExample : IExamplesProvider<UnitDto>
{
    public UnitDto GetExamples()
    {
        return new()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Kg",
            Status = true,
            MetaData = new()
            {
                CreatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                CreatedBy = "User",
                UpdatedBy = "User"
            }
        };
    }
}
