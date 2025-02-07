using AutoMapper;
using Contracts.Events;
using Ecoeden.Inventory.Application.Helpers;
using Ecoeden.Inventory.Domain.Entities.SQL;
using Ecoeden.Inventory.Domain.Models.Dtos;
using System.Globalization;

namespace Ecoeden.Inventory.Application.Mappers;
public class ProductStockMappingProfile: Profile
{
    public ProductStockMappingProfile()
    {
        CreateMap<ProductStock, StockDto>()
            .ForMember(d => d.MetaData, o => o.MapFrom(s => new MetadataDto
            {
                CreatedAt = new DateTimeOffset(DateTimeHelper.ConvertUtcToIst(s.CreatedAt), TimeSpan.Zero).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                UpdatedAt = new DateTimeOffset(DateTimeHelper.ConvertUtcToIst(s.UpdateAt), TimeSpan.Zero).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                CreatedBy = s.CreatedBy,
                UpdatedBy = s.UpdatedBy,
            }))
            .ReverseMap()
            .ForMember(s => s.Id, o => o.Ignore());

        CreateMap<ProductStock, ProductStockCreated>()
            .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.LastUpdatedOn, o => o.MapFrom(s => s.UpdateAt)).ReverseMap();

        CreateMap<ProductStock, ProductStockUpdated>()
            .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.LastUpdatedOn, o => o.MapFrom(s => s.UpdateAt)).ReverseMap();

        CreateMap<ProductStock, ProductStockDeleted>();
            
    }
}
