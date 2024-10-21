using AutoMapper;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Mappers;
public sealed class ContactDetailsMappingProfile : Profile
{
    public ContactDetailsMappingProfile()
    {
        CreateMap<ContactDetails, ContactDetailsDto>().ReverseMap();
        CreateMap<AddressDto, Address>().ReverseMap();
    }
}
