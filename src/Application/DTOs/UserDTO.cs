using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Application.DTOs;

    public class UserDto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;
        
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
        public string? Gender { get; set; }
        
        // Address Information
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string? Address { get; set; }
        
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string? City { get; set; }
        
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
        public string? State { get; set; }
        
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string? Country { get; set; }
        
        [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters")]
        public string? PostalCode { get; set; }
        
        // User Role and Status
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = "Guest"; // Guest, Admin, Manager, Staff
        
        public bool IsActive { get; set; } = true;
        
        public bool IsEmailVerified { get; set; } = false;
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime? UpdatedDate { get; set; }
        
        public DateTime? LastLoginDate { get; set; }
        
        // Profile Image URL
        public string? ProfileImageUrl { get; set; }
        
        // Computed Properties
        public string FullName => $"{FirstName} {LastName}";
        
        public int? Age => DateOfBirth.HasValue ? 
            DateTime.Now.Year - DateOfBirth.Value.Year - 
            (DateTime.Now.DayOfYear < DateOfBirth.Value.DayOfYear ? 1 : 0) : null;
    }

       public class RegisterUserDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;
        
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
        public string? Gender { get; set; }
        
        public string? Address { get; set; }
        
        public string? City { get; set; }
        
        public string? State { get; set; }
        
        public string? Country { get; set; }
        
        public string? PostalCode { get; set; }
    }
    
    // Login DTO
    public class LoginUserDto
    {
        [Required(ErrorMessage = "Username or email is required")]
        public string UsernameOrEmail { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
        
        public bool RememberMe { get; set; } = false;
    }
    
    // Update user profile DTO
    public class UpdateUserDto
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string? Username { get; set; }
        
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? Email { get; set; }
        
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string? FirstName { get; set; }
        
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string? LastName { get; set; }
        
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
        public string? Gender { get; set; }
        
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string? Address { get; set; }
        
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string? City { get; set; }
        
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
        public string? State { get; set; }
        
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string? Country { get; set; }
        
        [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters")]
        public string? PostalCode { get; set; }
        
        public string? ProfileImageUrl { get; set; }
    }
    
    // Change password DTO
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public string NewPassword { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
    
    // Admin user management DTO
    public class AdminUpdateUserDto
    {
        public string? Role { get; set; }
        
        public bool? IsActive { get; set; }
        
        public bool? IsEmailVerified { get; set; }
    }
    
    // Password reset request DTO
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; } = string.Empty;
    }
    
    // Reset password DTO
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Reset token is required")]
        public string Token { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public string NewPassword { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }