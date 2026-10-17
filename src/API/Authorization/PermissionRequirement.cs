using Microsoft.AspNetCore.Authorization;

namespace HotelManagementService.API.Authorization;
public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}