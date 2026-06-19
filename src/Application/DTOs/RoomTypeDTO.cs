using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Application.DTOs;
public record CreateRoomTypeDto(
   [Required] string Type,
   [Required] decimal Price,
   [Required] int Capacity
);
public record UpdateRoomTypeDto(
    int Id,
    string? Type,
    decimal? Price,
    string? Status
);
public record RoomTypeDto(
   int Id,
   string Type,
   decimal Price,
   int Capacity
);
