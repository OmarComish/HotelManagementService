using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;

public class User: BaseEntity
{
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
    public int DepartmentId { get; set; }
    public RecordStatus Status { get; set; } = RecordStatus.Active;

    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = null!; // Admin, Staff, Guest

    // Navigation properties
    public virtual Department Department { get; set; } = null!;
    public virtual Employee Employee { get; set; } = null!;
    
    
}