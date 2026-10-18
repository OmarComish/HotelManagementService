namespace HotelManagementService.Application.DTOs;
public record PermissionDto(
    string Key,
    string Name,
    string? Description,
    string Module);           // derived from the key prefix, e.g. "Users"