using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Application.DTOs;
public record CreateTaskDto(
    [Required] int HotelId,
    [Required] string Description,
    [Required] int TaskTypeId,
    int? AssignedToId,
    DateTime? DueDate,
    string? Notes,
    string Priority,
    int? Status,
    int RoomId,
    DateTime StartDate
);

public record UpdateTaskDto(
    string? Description,
    string? Type,
    int? AssignedToId,
    DateTime? DueDate,
    string? Status,
    string? Notes
);

public record TaskDto {
    public int Id {get; init;}
    public int HotelId {get; init;}
    public string RoomNumber {get; init;} = string.Empty;
    public string Description {get; init;} = string.Empty;
    public string Type {get; init;} = string.Empty;
    public int? AssignedToId {get; init;}
    public DateTime? DueDate {get; init;}
    public DateTime? StartDate {get; init;}
    public int Status {get; init;}
    public string? Notes {get; init;}= string.Empty;
    public DateTime? CreatedAt {get; init;}
    public string? AssignedToName {get; init;} = string.Empty;
    public int? Priority {get; init;} 
    public string? HotelName {get; init;}= string.Empty;
}
public class TaskStatusDto{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Description {get; set;}
}
public record TaskPrioritiesDto{
    public int Id {get; set;}
    public string? Name{get; set;}
    public string? Description {get; set;}
}
public record TaskTypeDto(
    int Id,
    string Name,
    string Title
);
public record CreateTaskTypeDto(
    [Required] string Name,
    [Required] string Title
);

public record UpdateTaskTypeDto(
    int Id,
    [Required] string Name,
    [Required] string Title
);