using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Application.DTOs;
public record CreateEmployeeDto(
  [Required] string FirstName,
  [Required] string LastName,
  [Required] int? DepartmentId
);

public record UpdateEmployeeDto(
   string FirstName,
   string LastName,
   int DepartmentId
);

public record EmployeeDto(
    int Id,
    string FirstName,
    string LastName,
    DepartmentDto Department
);

