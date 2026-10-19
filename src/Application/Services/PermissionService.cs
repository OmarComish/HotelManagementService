using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;
public class PermissionService: IPermissionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PermissionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserAccess?> GetAccessAsync(string username, CancellationToken ct = default)
    {
        var user = await _unitOfWork.Users.GetByUsernameWithRoleAsync(username, ct);
        if(user == null)
           return null;

        var role = user.AssignedRole;
        var fullName =  $"{user.FirstName} {user.LastName}";
        // Mirrors authorize() in rbac.ts: inactive users and inactive roles grant nothing.
        if(user.Status != RecordStatus.Active || role is null || !role.IsActive)
        {
            return new UserAccess(user.Username,null, Array.Empty<string>(), false, fullName);
        }

        var  permissions = role.RolePermissions
          .Select(rp => rp.Permission.Key)
          .ToList();

        return new UserAccess(user.Username, role.Key, permissions, role.IsSuperAdmin, fullName);

    }
}