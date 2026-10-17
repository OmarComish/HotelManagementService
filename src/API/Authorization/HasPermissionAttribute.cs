using Microsoft.AspNetCore.Authorization;

namespace HotelmanagementService.API.Authorization;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "Permission:";

    public HasPermissionAttribute(string permission)
        : base($"{PolicyPrefix}{permission}")
    {
    }
}