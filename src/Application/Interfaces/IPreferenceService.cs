using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface IPreferenceService
{
    Task<PreferenceDto> CreatePreferenceAsync(CreatePreferenceDto createPreferenceDto);
    Task<PreferenceDto?> GetPreferenceByIdAsync(int id);
    Task<IEnumerable<PreferenceDto>> GetAllPreferencesAsync();
    Task<PreferenceDto> UpdatePreferenceAsync(int id, UpdatePreferenceDto updatePreferenceDto);
    Task<bool> DeletePreferenceAsync(int id);
}