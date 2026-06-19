using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;
public class Preferences: BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

}