using System.ComponentModel.DataAnnotations;
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Application.DTOs;
public class CreateReservationDto
{
    [Required] 
    public int RoomId {get;set;}
    [Required] 
    public string FirstName {get;set;}
    [Required] 
    public string LastName {get;set;}
    [Required] 
    public string Email {get;set;}
    public string ReservationSource {get;set;}
    public string SpecialRequests {get;set;}
    public string Phone {get;set;}
    public string IdNumber {get;set;}
    [Required] 
    public DateTime CheckIn {get;set;}
    [Required] 
    public DateTime CheckOut {get;set;}
    [Required] 
    public int Guests {get;set;}
    public List<int> PreferenceIds {get;set;}= null; 
    public ReservationStatuses Status {get; set;} = ReservationStatuses.Reserved;
}

public record CreateWalkInCheckinDto(
    [Required] int RoomId,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string Email,
    string ReservationSource,
    string SpecialRequests,
    string Phone,
    string IdNumber,
    string IdType,
    string PaymentMethod,
    decimal Deposit,   
    [Required] DateTime CheckIn,
    [Required] DateTime CheckOut,
    [Required] int Guests,
    List<int> PreferenceIds = null 
);
public class ReservationDto
{
   
    public int Id {get; set;}
    public int RoomId {get; set;}
    public string GuestName {get; set;}
    public DateTime CheckIn {get; set;}
    public DateTime CheckOut {get; set;}
    public string Status {get; set;}
    public string ReservationSource {get; set;}
    public string SpecialRequests {get; set;}
    public string Phone {get; set;}
    public decimal TotalAmount {get; set;}
    public string Email {get; set;}
    public int Guests {get; set;}
    public DateTime CreatedAt {get; set;}
    public RoomDto? Room {get; set;} = null;

}
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