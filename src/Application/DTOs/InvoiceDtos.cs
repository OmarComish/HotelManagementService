using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
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
public record LineItemsDto
{
    [Required] 
    public int InvoiceId {get; set;}
    [Required] 
    public string Description {get; set;}
    [Required] 
    public int Quantity {get; set;}
    [Required] 
    public decimal UnitPrice {get; set;}
    public decimal LineTotal {get; set;}
}

public class ReadInvoiceDto
{
    public int ReservationId {get; set;}
    public string InvoiceNumber {get; set;}
    public DateTime CheckIn {get; set;}
    public DateTime CheckOut {get; set;}
    public string Status {get; set;}
    public decimal TotalAmount {get; set;}
    public string RoomNumber {get; set;}
    public string Guest {get; set;}
    public string? PaymentMethod {get; set;}
    public List<LineItemsDto> LineItems {get; set;} = new();
}

public record MarkInvoiceAsPaidDto(
    [Required] string InvoiceNumber,
    [Required] string PaymentMethod,
    string? Notes = null
);

public record ChangeInvoiceStatusDto(
    [Required] string InvoiceNumber,
    [Required] InvoiceStatus NewStatus
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