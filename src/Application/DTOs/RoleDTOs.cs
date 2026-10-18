namespace HotelManagementService.Application.DTOs;
public record RoleDto(
    int Id,
    string Key,
    string Name,
    string? Description,
    string Status,            // "active" | "inactive", same values as rbac.ts
    bool IsSystem,
    bool IsSuperAdmin,
    IReadOnlyList<string> Permissions);