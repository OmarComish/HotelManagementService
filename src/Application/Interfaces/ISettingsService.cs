using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface ISettingsService
{
    Task<SettingsDto> GetSettingsAsync(CancellationToken ct = default);
    Task<SecuritySettingsDto> GetSecurityAsync(CancellationToken ct = default);
    Task<GeneralSettingsDto> UpdateGeneralAsync(string actorUsername, GeneralSettingsDto input, CancellationToken ct = default);
    Task<SecuritySettingsDto> UpdateSecurityAsync(string actorUsername, SecuritySettingsDto input, CancellationToken ct = default);
}