using System.ComponentModel.DataAnnotations;
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Application.DTOs;
public record CreateRoomDto(
    [Required] int HotelId,
    [Required] string RoomNumber,
    [Required] int RoomTypeId,
    [Required] decimal Price,
    [Required] int Capacity
    //[Required] int AmenitiesId 
);

public record UpdateRoomDto(
    int Type,
    decimal? Price,
    string? Status
);

public record RoomDto
{
     public int Id {get; init;}
     public int HotelId {get; init;}
     public string RoomNumber {get; init;}
     public string Type {get; init;} 
     public decimal Price {get; init;}
     public string Status {get; init;}
     public IEnumerable<AmenitiesDto> Amenitieslist {get; set;} = new List<AmenitiesDto>();
     public string? HotelName { get; init; }
     
}
 
    
