namespace HotelManagementService.Core.Entities;
public record UpdateUserRequest(string FirstName, string LastName, string Email);
public record CreateUserRequest(
    string FirstName, string LastName, string Username, string Email,
    string Password, string RoleKey, int DepartmentId);

public record ResetPasswordRequest(string NewPassword);