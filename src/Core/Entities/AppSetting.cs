using HotelManagementService.Core.Entities;

public class AppSetting : BaseEntity
{
    public string Section { get; set; } = null!;   // "general" | "security"
    public string Value { get; set; } = null!;     // JSON
}