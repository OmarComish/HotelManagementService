using System.ComponentModel.DataAnnotations;
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Application.DTOs;
public record CreateRoomDto(
    [Required] int HotelId,
    [Required] string RoomNumber,
    [Required] int RoomTypeId,
    [Required] decimal Price,
    [Required] int Capacity,
    [Required] int[] AmenitiesId 
);

public record UpdateRoomDto(
    int Type,
    decimal? Price,
    string? Status
);

public class RoomDto
{
     public int Id {get; set;}
     public int HotelId {get; set;}
     public string RoomNumber {get; set;}
     public string Type {get; set;} 
     public decimal Price {get; set;}
     public string Status {get; set;}
     public IEnumerable<AmenitiesDto> Amenitieslist {get; set;} = new List<AmenitiesDto>();
     public string? HotelName { get; set; }
     
}
 
    
