using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface IGuestPreferenceService
{
    Task<GuestPreferenceDto> CreateGuestPreferenceAsync(CreateGuestPreferenceDto createGuestPreferenceDto);
    Task<GuestPreferenceDto?> GetGuestPreferenceByIdAsync(int id);
    Task<IEnumerable<GuestPreferenceDto>> GetAllGuestPreferencesAsync();
    Task<GuestPreferenceDto> UpdateGuestPreferenceAsync(int id, UpdateGuestPreferenceDto updateGuestPreferenceDto);
    Task<bool> DeleteGuestPreferenceAsync(int id);
}