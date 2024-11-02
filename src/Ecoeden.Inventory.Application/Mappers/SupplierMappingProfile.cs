using AutoMapper;
using Contracts.Events;
using Ecoeden.Inventory.Application.Helpers;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Dtos;
using System.Globalization;

namespace Ecoeden.Inventory.Application.Mappers;
public sealed class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<Supplier, SupplierDto>()
            .ForMember(d => d.ContactDetails, o => o.MapFrom(s => s.ContactDetails))
            .ForMember(d => d.Metadata, o => o.MapFrom(s => new MetadataDto
            {
                CreatedAt = new DateTimeOffset(DateTimeHelper.ConvertUtcToIst(s.CreatedAt), TimeSpan.Zero).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                UpdatedAt = new DateTimeOffset(DateTimeHelper.ConvertUtcToIst(s.UpdatedAt), TimeSpan.Zero).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                CreatedBy = s.CreatedBy,
                UpdatedBy = s.UpdatedBy,
            })).ReverseMap();


        CreateMap<Supplier, SupplierCreated>()
            .ForMember(d => d.Email, o => o.MapFrom(s => s.ContactDetails.Email))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.ContactDetails.Phone))
            .ForMember(d => d.Address, o => o.MapFrom(s => s.ContactDetails.Address.GetAddressString()))
            .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.LastUpdatedOn, o => o.MapFrom(s => s.UpdatedAt)).ReverseMap();

        CreateMap<Supplier, SupplierUpdated>()
            .ForMember(d => d.Email, o => o.MapFrom(s => s.ContactDetails.Email))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.ContactDetails.Phone))
            .ForMember(d => d.Address, o => o.MapFrom(s => s.ContactDetails.Address.GetAddressString()))
            .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.LastUpdatedOn, o => o.MapFrom(s => s.UpdatedAt)).ReverseMap();

        CreateMap<Supplier, SupplierDeleted>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));
    }
}
