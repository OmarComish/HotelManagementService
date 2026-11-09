namespace HotelManagementService.Application.Interfaces;
public record UserAccess(
    string Username,
    string? RoleKey,
    IReadOnlyCollection<string> Permissions,
    bool IsSuperAdmin = false,
    string? FullName = null,
    int UserId = 0,
    string? Email = null);
public interface IPermissionService
{
    /// Returns null if the user does not exist. Returns an empty permission set
    /// if the user is not active, has no role, or the role is inactive.
    Task<UserAccess?> GetAccessAsync(string username, CancellationToken ct = default);
}