namespace HotelManagementService.Application.Interfaces;

public record IdentityOperationResult(bool Succeeded, string? Error);

public interface IIdentityAdminClient
{
    Task<IdentityOperationResult> CreateAccountAsync(string username, string email, string password, CancellationToken ct = default);
    Task<IdentityOperationResult> ResetPasswordAsync(string username, string newPassword, CancellationToken ct = default);
    Task<IdentityOperationResult> DeleteAccountAsync(string username, CancellationToken ct = default);
}