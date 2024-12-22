using Ecoeden.Inventory.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Unit;
public class UnitListResponseExample : IExamplesProvider<List<UnitDto>>
{
    public List<UnitDto> GetExamples()
    {
        return 
        [
            new() 
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Kg",
                Status = true,
                MetaData = new(){
                    CreatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    CreatedBy = "User 1",
                    UpdatedBy = "User 1"
                } 
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Lt",
                Status = true,
                MetaData = new(){
                    CreatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    CreatedBy = "User 2",
                    UpdatedBy = "User 2"
                }
            },
       ];
    }
}
