using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Application.Contracts.Security;
public interface IPermissionMapper
{
    List<string> GetPermissionsForRole(ApiAccess role);
}
