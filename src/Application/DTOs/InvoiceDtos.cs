using System.ComponentModel.DataAnnotations;
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Application.DTOs;
public record CreateInvoiceDto (
    [Required] int ReservationId,
    [Required] string InvoiceNumber,
    [Required] DateTime IssuedAt,
    [Required] InvoiceStatus Status,
    List<LineItemsDto> LineItems
);
public record InvoiceDto(
    int ReservationId,
    string InvoiceNumber,
    DateTime IssuedAt,
    InvoiceStatus Status,
    List<LineItemsDto>? LineItems = null
);
public record LineItemsDto(
    [Required] int InvoiceId,
    [Required] string Description,
    [Required] int Quantity,
    [Required] decimal UnitPrice,
    decimal LinetTotal
);
/*
// Invoice DTOs (referenced but not defined)
public record CreateInvoiceDto(
    [Required] int BookingId,
    [Required] decimal TotalAmount,
    [Required] decimal Tax,
    DateTime? DueDate
);

public record UpdateInvoiceDto(
    decimal? TotalAmount,
    decimal? Tax,
    DateTime? DueDate,
    string? Status
);

public record InvoiceDto(
    int Id,
    int BookingId,
    decimal TotalAmount,
    decimal Tax,
    DateTime IssuedDate,
    DateTime? DueDate,
    string Status,
    BookingDto? Booking = null
);
*/