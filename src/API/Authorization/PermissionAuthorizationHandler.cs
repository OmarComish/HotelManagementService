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

        Console.WriteLine($"Username: {username}");
        Console.WriteLine($"Access found: {access != null}");
        Console.WriteLine($"Role: {access?.RoleKey}");
        Console.WriteLine($"IsSuperAdmin: {access?.IsSuperAdmin}");
        Console.WriteLine($"Permission: {requirement.Permission}");

        if (access is null)
            return;

        if (access.IsSuperAdmin || access.Permissions.Contains(requirement.Permission))
        {
            Console.WriteLine("PERMISSION GRANTED");
            context.Succeed(requirement);
        }
        else
        {
            Console.WriteLine("PERMISSION DENIED");
        }
            
    }
}