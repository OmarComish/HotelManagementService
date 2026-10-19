using System.Text.Json;

namespace HotelManagementService.Application.DTOs;

public record AuditEntry(
    string ActorUsername,
    string ActorName,
    string Action,
    string EntityType,
    string? EntityId,
    string EntityLabel,
    object? Previous = null,
    object? New = null);

public record AuditLogDto(
    int Id,
    string ActorUsername,
    string ActorName,
    string Action,
    string Entity,
    string? EntityId,
    string EntityLabel,
    DateTime Timestamp,
    string? IpAddress,
    string? UserAgent,
    JsonElement? PreviousValue,
    JsonElement? NewValue);