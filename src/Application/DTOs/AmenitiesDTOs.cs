using System.ComponentModel.DataAnnotations;
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Application.DTOs;
public record AmenitiesDto {
    public int Id { get; init; }
    public required string Name { get; init; }
}
public record CreateAmenitiesDto {
    [Required]
    public required string Name { get; init; }
}
public record UpdateAmenitiesDto {
    public int Id { get; init; }
    public required string Name { get; init; }
}
