using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Application.DTOs;
public record CreateGuestDto {
    [Required] 
    public string FirstName {get;init;}
    [Required] 
    public string LastName {get;init;}
    [Required] 
    public string PhoneNumber {get;init;}
    [Required] 
    public string Address {get;init;}
    [Required] 
    public string IDNumber {get;init;}
    public string Email {get;init;}
    public string Notes {get;init;}
    public List<int> PreferenceIds {get;init;} // list of selected preferences
    //CreateBookingDto? Booking = null //optional booking
}

public record UpdateGuestDto(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string PhoneNumber,
    [Required] string Address,
    string? Email,
    [Required]string IDNumber
);

public record GuestDto {
    public int Id {get; init;}    
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string IDNumber { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Notes { get; init; }
    DateTime CreatedAt {get; set;}
    public IEnumerable<string> Preferences { get; init; } = new List<string>();
    public IEnumerable<GuestBookingDto> Bookings { get; init; } = new List<GuestBookingDto>();
};
public record GuestBookingDto
{
    public int Id { get; init; }
    public int RoomNumber { get; init; }
    public DateTime CheckIn { get; init; }
    public DateTime CheckOut { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
}