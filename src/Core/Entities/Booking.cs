using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementService.Core.Entities;
public class Booking : BaseEntity
{
    [Required]
    public int RoomId { get; set; }

    [Required]
    public int GuestId { get; set; }

    [Required]
    public DateTime CheckIn { get; set; }

    [Required]
    public DateTime CheckOut { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Reserved";

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    // Navigation properties
    public virtual Room Room { get; set; } = null!;
    public virtual Guest Guest { get; set; } = null!;
    public virtual Payment? Payment { get; set; }
    public virtual Invoice? Invoice { get; set; }
}