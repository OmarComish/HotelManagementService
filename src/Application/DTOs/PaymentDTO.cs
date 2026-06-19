using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Application.DTOs;
public record CreatePaymentDto(
    [Required] int BookingId,
    [Required] decimal Amount,
    [Required] string PaymentMethod
);

public record UpdatePaymentDto(
    decimal? Amount,
    string? Status,
    string? PaymentMethod,
    DateTime? ProcessedAt
);

public record PaymentDto(
    int Id,
    int BookingId,
    decimal Amount,
    string Status,
    string PaymentMethod,
    DateTime? ProcessedAt,
    DateTime CreatedAt,
    BookingDto? Booking = null
);