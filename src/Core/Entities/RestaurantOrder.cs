using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementService.Core.Entities;
public class RestaurantOrder : BaseEntity
{
    [Required]
    public int TableId { get; set; }

    [Required]
    public int OrderTypeId { get; set; }
    public int? GuestId { get; set; }
    public int? ReservationId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "New";

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(500)]
    public string? SpecialInstructions { get; set; }

    // Navigation properties
    public virtual RestaurantTable Table { get; set; } = null!;

    public virtual OrderType OrderType { get; set; } = null!;
    public virtual Guest? Guest { get; set; }
    public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public virtual Reservation? Reservation { get; set; }  
}