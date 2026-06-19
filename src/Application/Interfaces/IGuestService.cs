using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface IGuestService
{
    Task<GuestDto> CreateGuestAsync(CreateGuestDto createGuestDto);
    Task<GuestDto?> GetGuestByIdAsync(int id);
    Task<IEnumerable<GuestDto>> GetAllGuestsAsync();
    Task<GuestDto> UpdateGuestAsync(int id, UpdateGuestDto updateGuestDto);
    Task<bool> DeleteGuestAsync(int id);
}