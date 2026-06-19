using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementService.Core.Entities;

public class Room : BaseEntity
{
    [Required]
    public int HotelId { get; set; }

    [Required]
    [MaxLength(10)]
    public string RoomNumber { get; set; } = null!;

    [Required]
    public int RoomTypeId { get; set; } 

    [Required]
    public RecordStatus Status { get; set; }

    // Navigation properties
    [ForeignKey(nameof(HotelId))]
    public virtual Hotel Hotel { get; set; } = null!;
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public virtual ICollection<TaskItem> TaskItems {get; set;} = new List<TaskItem>();

    [ForeignKey(nameof(RoomTypeId))]
    public virtual RoomType RoomType { get; set; } = null!;
   
   
}