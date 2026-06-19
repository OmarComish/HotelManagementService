using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Core.Entities;
public class Guest : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

   
    [MaxLength(50)]
    public string? Email { get; set; } 

    [Required]
    [MaxLength(50)]
    public string? PhoneNumber { get; set; }

    
    [MaxLength(50)]
    public string? Address { get; set; }

     
    [MaxLength(50)]
    public string? IDNumber { get; set; }

     
    [MaxLength(255)]
    public string? Notes { get; set; }

    // Navigation properties
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<RestaurantOrder> RestaurantOrders { get; set; } = new List<RestaurantOrder>();
    public virtual ICollection<GuestPreferences> GuestPreferences { get; set; } = new List<GuestPreferences>();
}