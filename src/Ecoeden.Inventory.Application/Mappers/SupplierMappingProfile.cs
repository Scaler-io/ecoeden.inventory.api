using AutoMapper;
using Ecoeden.Inventory.Application.Helpers;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Mappers;
public sealed class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<Supplier, SupplierDto>()
            .ForMember(d => d.ContactDetails, o => o.MapFrom(s => s.ContactDetails))
            .ForMember(d => d.Metadata, o => o.MapFrom(s => new MetadataDto
            {
                CreatedAt = DateTimeHelper.ConvertUtcToIst(s.CreatedAt).ToString("dd/MM/yyyy HH:mm:ss tt"),
                UpdatedAt = DateTimeHelper.ConvertUtcToIst(s.UpdatedAt).ToString("dd/MM/yyyy HH:mm:ss tt"),
                CreatedBy = s.CreatedBy,
                UpdatedBy = s.UpdatedBy,
            })).ReverseMap();

    }
}
