using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Application.DTOs;
public record CreateDepartmentDto(
    [Required] string Name
);
public record DepartmentDto(
   int Id,
   string Name
);