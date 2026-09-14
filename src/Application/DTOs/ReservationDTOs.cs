using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Application.DTOs;
public record CreateReservationDto(
    [Required] int RoomId,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    string ReservationSource,
    string SpecialRequests,
    string Phone,
    string IdNumber,   
    [Required] DateTime CheckIn,
    [Required] DateTime CheckOut,
    [Required] int Guests,
    List<int> PreferenceIds = null 
);
public record ReservationDto(
    int Id,
    int RoomId,
    string GuestName,
    DateTime CheckIn,
    DateTime CheckOut,
    string Status,
    string ReservationSource,
    string SpecialRequests,
    string Phone,
    decimal TotalAmount,
    string Email,
    int Guests,
    DateTime CreatedAt,
    RoomDto? Room = null
);
/*public record UpdateReservationDto(
    int Id,
    int? RoomId,
    string? GuestName,
    DateTime? CheckIn,
    DateTime? CheckOut,
    string? Status,
    string? ReservationSource,
    string? SpecialRequests,
    string? Phone,
    decimal? TotalAmount,
    string? Email,
    int? Guests
);*/
public class UpdateReservationDto
{
    public int Id { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public string? SpecialRequests { get; set; }
    public string? ReservationSource { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Status { get; set; }
    public string? GuestName { get; set; }
    public int? RoomId { get; set; }
}
/*public record CheckInDto(
    [Required]int ReservationId,
    string PaymentMethod,
    [Required]decimal DepositAmount,
    string SpecialRequests,
    DateTime? CheckIn,
    DateTime? CheckOut
);*/
public class CheckInDto
{
     public int ReservationId { get; set; }
     public DateTime? CheckIn { get; set; }
     public DateTime? CheckOut { get; set; }
     public string? SpecialRequests { get; set; }
     public string? PaymentMethod {get; set;}
   /* public string? ReservationSource { get; set; }
   
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Status { get; set; }
    public string? GuestName { get; set; }
    public int? RoomId { get; set; }*/
}