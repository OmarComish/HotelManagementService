namespace HotelManagementService.Application.DTOs;
public record GeneralSettingsDto(
    string PropertyName, string ContactEmail, string ContactPhone,
    string Timezone, string Currency, string CheckInTime, string CheckOutTime);

public record SecuritySettingsDto(
    int PasswordMinLength, bool RequireStrongPassword, int MaxFailedAttempts,
    int SessionTimeoutMinutes, bool LockOnFailedAttempts);

public record SettingsDto(GeneralSettingsDto General, SecuritySettingsDto Security);