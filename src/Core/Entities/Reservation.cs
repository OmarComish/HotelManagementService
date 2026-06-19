using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HotelManagementService.Core.Entities;

public class Reservation : BaseEntity
{
    [Required]
    public int RoomId { get; set; }

    [Required]
    public string? GuestName { get; set; }

    [Required]
    public DateTime CheckIn { get; set; }

    [Required]
    public DateTime CheckOut { get; set; }

    [Required]
    [MaxLength(20)]
    public ReservationStatuses Status { get; set; } 

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string ReservationSource { get; set; } = null!;

    [MaxLength(250)]
    public string SpecialRequests { get; set; } = null!;
    [Required]
    public string Phone { get; set; } = null!;

    [Required]
    public int Guests { get; set; }

    // Navigation properties
    public virtual Room Room { get; set; } = null!;
    //public virtual Guest Guest { get; set; } = null!;
    //public virtual Payment? Payment { get; set; }
    //public virtual Invoice? Invoice { get; set; }
}