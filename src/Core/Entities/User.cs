using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;

public class User: BaseEntity
{

    [Required]
    public string Username {get; set;} = null!;
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;
    public RecordStatus Status { get; set; } = RecordStatus.Active;
    public int EmployeeId {get; set;}

    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = null!; // Admin, Staff, Guest

    public int? RoleId {get; set;}

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
    public virtual Role? AssignedRole {get; set;}
    
    
}