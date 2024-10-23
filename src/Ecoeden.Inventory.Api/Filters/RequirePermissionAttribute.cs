using Ecoeden.Inventory.Api.Services;
using Ecoeden.Inventory.Application.Contracts.Security;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecoeden.Inventory.Api.Filters;

public class RequirePermissionAttribute : TypeFilterAttribute
{
    public RequirePermissionAttribute(ApiAccess requiredPermission) : base(typeof(RequirePermissionExecutor))
    {
        Arguments = [requiredPermission];
    }

    private class RequirePermissionExecutor(ApiAccess role,
        IPermissionMapper permissionMapper,
        IIdentityService identityService,
        ILogger logger) : Attribute, IActionFilter
    {

        private readonly ILogger _logger = logger;
        private readonly IIdentityService _identityService = identityService;
        private readonly IPermissionMapper _permissionMapper = permissionMapper;

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.Here().MethodEntered();
            var currentUser = _identityService.PrepareUser();
            List<string> requiredPermissions = [.. _permissionMapper.GetPermissionsForRole(role)];

            var commonPermissions = requiredPermissions.Intersect(currentUser.AuthorizationDto.Permissions).ToList();

            if (commonPermissions.Count == 0)
            {
                _logger.Here().Error("No matching permission found");
                context.Result = new UnauthorizedObjectResult(new ApiResponse(ErrorCodes.Unauthorized, "Access denied"));
            }

            _logger.Here().MethodExited();
        }
    }
}
