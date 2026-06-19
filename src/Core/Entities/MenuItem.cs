using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementService.Core.Entities;
public class MenuItem : BaseEntity
{
    [Required]
    public int HotelId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = null!; // food, drink, etc.

    public bool IsAvailable { get; set; } = true;

    // Navigation properties
    public virtual Hotel Hotel { get; set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}