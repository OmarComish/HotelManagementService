using HotelManagementService.API.Authorization;
using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HotelmanagementService.API.Authorization;
public class PermissionAuthorizationHandler: AuthorizationHandler<PermissionRequirement>
{
     private readonly IPermissionService _permissionService;
    public PermissionAuthorizationHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }
     protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var username = context.User.FindFirst("username")?.Value;
        if (string.IsNullOrWhiteSpace(username))
            return; // no Succeed => 403

        var access = await _permissionService.GetAccessAsync(username);
        if (access is null)
            return;

        if (access.Permissions.Contains(requirement.Permission))
            context.Succeed(requirement);
    }
}