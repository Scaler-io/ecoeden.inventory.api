using Ecoeden.Inventory.Application.Contracts.Security;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Infrastructure.Security;
public class PermissionMapper : IPermissionMapper
{
    private readonly Dictionary<ApiAccess, List<string>> _map = new()
    {
        { ApiAccess.InventoryRead, [ApiAccess.InventoryRead.EnumValueString()] },
        { ApiAccess.ReportRead, [ ApiAccess.ReportRead.EnumValueString()] },
        { ApiAccess.ReportWrite, [ ApiAccess.ReportRead.EnumValueString(), ApiAccess.ReportWrite.EnumValueString()] },
        { ApiAccess.InventoryWrite, [ ApiAccess.InventoryWrite.EnumValueString()] }
    };

    public List<string> GetPermissionsForRole(ApiAccess role)
    {
        return _map[role];
    }
}
