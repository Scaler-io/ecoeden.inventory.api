using AutoMapper;
using Contracts.Events;
using Ecoeden.Inventory.Application.Helpers;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Dtos;
using System.Globalization;

namespace Ecoeden.Inventory.Application.Mappers;
public sealed class UnitMappingProfile : Profile
{
    public UnitMappingProfile()
    {
        CreateMap<Unit, UnitDto>()
            .ForMember(d => d.MetaData, o => o.MapFrom(s => new MetadataDto
            {
                CreatedAt = new DateTimeOffset(DateTimeHelper.ConvertUtcToIst(s.CreatedAt), TimeSpan.Zero).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                UpdatedAt = new DateTimeOffset(DateTimeHelper.ConvertUtcToIst(s.UpdatedAt), TimeSpan.Zero).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                CreatedBy = s.CreatedBy,
                UpdatedBy = s.UpdatedBy,
            })).ReverseMap();

        CreateMap<Unit, UnitCreated>()
            .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.LastUpdatedOn, o => o.MapFrom(s => s.UpdatedAt)).ReverseMap();
        CreateMap<Unit, UnitUpdated>()
            .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.LastUpdatedOn, o => o.MapFrom(s => s.UpdatedAt)).ReverseMap();
        CreateMap<Unit, UnitDeleted>()
            .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.LastUpdatedOn, o => o.MapFrom(s => s.UpdatedAt)).ReverseMap();
    }
}
