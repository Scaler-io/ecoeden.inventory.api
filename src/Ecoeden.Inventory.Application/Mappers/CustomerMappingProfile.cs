using AutoMapper;
using Ecoeden.Inventory.Application.Helpers;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Dtos;
using System.Globalization;

namespace Ecoeden.Inventory.Application.Mappers;
public sealed class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(d => d.ContactDetails, o => o.MapFrom(s => s.ContactDetails))
            .ForMember(d => d.MetaData, o => o.MapFrom(s => new MetadataDto
            {
                CreatedAt = new DateTimeOffset(DateTimeHelper.ConvertUtcToIst(s.CreatedAt), TimeSpan.Zero).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                UpdatedAt = new DateTimeOffset(DateTimeHelper.ConvertUtcToIst(s.UpdatedAt), TimeSpan.Zero).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                CreatedBy = s.CreatedBy,
                UpdatedBy = s.UpdatedBy,
            })).ReverseMap();
    }
}
