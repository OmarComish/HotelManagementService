using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.RegularExpressions;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Exceptions;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;

public class SettingsService : ISettingsService
{
    private const string GeneralSection = "general";
    private const string SecuritySection = "security";

    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private static readonly GeneralSettingsDto DefaultGeneral =
        new("Grand Hotel", "info@hotel.com", "", "UTC", "USD", "15:00", "11:00");

    private static readonly SecuritySettingsDto DefaultSecurity = new(8, true, 5, 60, true);

    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    private readonly IAuditService _audit;

    public SettingsService(IUnitOfWork unitOfWork, IPermissionService permissionService, IAuditService audit)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
        _audit = audit;
    }

    public async Task<SettingsDto> GetSettingsAsync(CancellationToken ct = default) =>
        new(await LoadAsync(GeneralSection, DefaultGeneral), await LoadAsync(SecuritySection, DefaultSecurity));

    public Task<SecuritySettingsDto> GetSecurityAsync(CancellationToken ct = default) =>
        LoadAsync(SecuritySection, DefaultSecurity);

    public async Task<GeneralSettingsDto> UpdateGeneralAsync(
        string actorUsername, GeneralSettingsDto input, CancellationToken ct = default)
    {
        var actor = await GetActorAsync(actorUsername, ct);

        var propertyName = (input.PropertyName ?? string.Empty).Trim();
        if (propertyName.Length == 0)
            throw new RbacException(422, "Property name is required");

        var email = (input.ContactEmail ?? string.Empty).Trim().ToLowerInvariant();
        if (email.Length == 0 || !new EmailAddressAttribute().IsValid(email))
            throw new RbacException(422, "A valid contact email is required");

        ValidateTime(input.CheckInTime, "Check-in time");
        ValidateTime(input.CheckOutTime, "Check-out time");

        var previous = await LoadAsync(GeneralSection, DefaultGeneral);
        var updated = new GeneralSettingsDto(
            Truncate(propertyName, 120),
            email,
            Truncate((input.ContactPhone ?? string.Empty).Trim(), 30),
            Truncate((input.Timezone ?? string.Empty).Trim(), 60),
            Truncate((input.Currency ?? string.Empty).Trim(), 10),
            input.CheckInTime,
            input.CheckOutTime);

        await SaveAsync(GeneralSection, updated, actor.Username);
        await _audit.RecordAsync(new AuditEntry(
            actor.Username, actor.FullName ?? actor.Username,
            "settings.updated", "settings", "general", "General settings",
            previous, updated));
        await _unitOfWork.SaveChangesAsync();

        return updated;
    }

    public async Task<SecuritySettingsDto> UpdateSecurityAsync(
        string actorUsername, SecuritySettingsDto input, CancellationToken ct = default)
    {
        var actor = await GetActorAsync(actorUsername, ct);

        var previous = await LoadAsync(SecuritySection, DefaultSecurity);
        var updated = new SecuritySettingsDto(
            Math.Clamp(input.PasswordMinLength, 6, 24),
            input.RequireStrongPassword,
            Math.Clamp(input.MaxFailedAttempts, 3, 20),
            Math.Clamp(input.SessionTimeoutMinutes, 5, 1440),
            input.LockOnFailedAttempts);

        await SaveAsync(SecuritySection, updated, actor.Username);
        await _audit.RecordAsync(new AuditEntry(
            actor.Username, actor.FullName ?? actor.Username,
            "settings.updated", "settings", "security", "Security settings",
            previous, updated));
        await _unitOfWork.SaveChangesAsync();

        return updated;
    }

    /* ---------- helpers ---------- */

    private async Task<T> LoadAsync<T>(string section, T defaults) where T : class
    {
        var row = await _unitOfWork.AppSettings.FirstOrDefaultAsync(s => s.Section == section);
        return row is null
            ? defaults
            : JsonSerializer.Deserialize<T>(row.Value, JsonOptions) ?? defaults;
    }

    private async Task SaveAsync<T>(string section, T value, string actor)
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        var row = await _unitOfWork.AppSettings.FirstOrDefaultAsync(s => s.Section == section);

        if (row is null)
        {
            await _unitOfWork.AppSettings.AddAsync(
                new AppSetting { Section = section, Value = json, CreatedBy = actor });
        }
        else
        {
            row.Value = json;
            row.UpdatedAt = DateTime.UtcNow;
            row.UpdatedBy = actor;
        }
    }

    private async Task<UserAccess> GetActorAsync(string username, CancellationToken ct) =>
        await _permissionService.GetAccessAsync(username, ct)
            ?? throw new RbacException(401, "Unauthorized");

    private static void ValidateTime(string? value, string label)
    {
        if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^([01]\d|2[0-3]):[0-5]\d$"))
            throw new RbacException(422, $"{label} must be in HH:mm format");
    }

    private static string Truncate(string value, int max) =>
        value.Length <= max ? value : value[..max];
}