using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Api.Services;

public interface IIdentityService
{
    UserDto PrepareUser();
}
