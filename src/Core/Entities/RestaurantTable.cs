using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;
public class RestaurantTable : BaseEntity
{
    [Required]
    public int HotelId { get; set; }

    [Required]
    [MaxLength(10)]
    public string TableNumber { get; set; } = null!;

    [Required]
    public int Capacity { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Available";

    // Navigation properties
    public virtual Hotel Hotel { get; set; } = null!;
    public virtual ICollection<RestaurantOrder> Orders { get; set; } = new List<RestaurantOrder>();
}