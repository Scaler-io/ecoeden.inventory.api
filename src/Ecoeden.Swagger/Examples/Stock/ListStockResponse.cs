using Ecoeden.Inventory.Domain.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Ecoeden.Swagger.Examples.Stock;
public class ListStockResponse : IExamplesProvider<List<StockDto>>
{
    public List<StockDto> GetExamples()
    {
        return [
            new(){
                Id = Guid.NewGuid().ToString(),
                SupplierId = Guid.NewGuid().ToString(),
                ProductId = Guid.NewGuid().ToString(),
                Quantity = 1,
                MetaData = new()
                {
                    CreatedAt = DateTime.UtcNow.ToString(format: "dd/MM/yyyy HH:mm:ss tt"),
                    UpdatedAt = DateTime.UtcNow.ToString(format: "dd/MM/yyyy HH:mm:ss tt"),
                    CreatedBy = Guid.NewGuid().ToString(),
                    UpdatedBy = Guid.NewGuid().ToString(),
                }
            },
            new(){
                Id = Guid.NewGuid().ToString(),
                SupplierId = Guid.NewGuid().ToString(),
                ProductId = Guid.NewGuid().ToString(),
                Quantity = 1,
                MetaData = new()
                {
                    CreatedAt = DateTime.UtcNow.ToString(format: "dd/MM/yyyy HH:mm:ss tt"),
                    UpdatedAt = DateTime.UtcNow.ToString(format: "dd/MM/yyyy HH:mm:ss tt"),
                    CreatedBy = Guid.NewGuid().ToString(),
                    UpdatedBy = Guid.NewGuid().ToString(),
                }
            }
        ];
    }
}
