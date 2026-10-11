using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface IAuthenticationService
{
    Task<AuthResponseDto> AuthenticateUser(LogInRequestDto dto);
    Task<ResponseDto> RegisterUser(RegisterUserDto dto);
}