namespace HotelManagementService.Core.Entities;

public class Permission : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    //Navigation property
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}