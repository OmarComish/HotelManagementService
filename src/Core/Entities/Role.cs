namespace HotelManagementService.Core.Entities;
public class Role: BaseEntity
{
    public string Key { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsSystem { get; set; }

    public bool IsSuperAdmin { get; set; }

    // Navigation property
    public ICollection<RolePermission> RolePermissions { get; set; }
        = new List<RolePermission>();
}