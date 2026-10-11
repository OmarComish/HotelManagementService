using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface IApiService
{
    Task<TokenResponseDto> GetTokenAsync(TokenRequestDto request);
    Task<ResponseDto> RegisterUserAsync(RegisterUserDto request);
    Task<AuthResponseDto> SignInAsync(LogInRequestDto request);
}