using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementService.Core.Entities;
public class OrderItem : BaseEntity
{
    [Required]
    public int OrderId { get; set; }

    [Required]
    public int MenuItemId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [MaxLength(200)]
    public string? Notes { get; set; }

    // Navigation properties
    public virtual RestaurantOrder Order { get; set; } = null!;
    public virtual MenuItem MenuItem { get; set; } = null!;
}