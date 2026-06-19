using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;

public class User: BaseEntity
{
   [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = null!; // Admin, Staff, Guest

    // Navigation properties
    
}