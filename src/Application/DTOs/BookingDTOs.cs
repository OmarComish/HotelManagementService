using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Application.DTOs;
public record CreateBookingDto(
    [Required] int RoomId,
    [Required] int GuestId,
    [Required] DateTime CheckIn,
    [Required] DateTime CheckOut
);

public record UpdateBookingDto(
    DateTime? CheckIn,
    DateTime? CheckOut,
    string? Status
);

public record BookingDto(
    int Id,
    int RoomId,
    int GuestId,
    DateTime CheckIn,
    DateTime CheckOut,
    string Status,
    decimal TotalAmount,
    DateTime CreatedAt,
    RoomDto? Room = null,
    GuestDto? Guest = null
);