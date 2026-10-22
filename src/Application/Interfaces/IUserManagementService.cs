using HotelManagementService.Application.DTOs;
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Application.Interfaces;
public interface IUserManagementService
{
    Task<PagedResult<ManagedUserDto>> GetUsersAsync(UserQuery query, CancellationToken ct = default);
    Task<ManagedUserDto?> GetUserAsync(int id, CancellationToken ct = default);
    Task<ManagedUserDto> ChangeRoleAsync(string actorUsername, int id, string roleKey, CancellationToken ct = default);
    Task<ManagedUserDto> SetStatusAsync(string actorUsername, int id, string status, CancellationToken ct = default);
    Task DeleteUserAsync(string actorUsername, int id, CancellationToken ct = default);
    Task<ManagedUserDto> UpdateUserAsync(string actorUsername, int id, UpdateUserRequest request, CancellationToken ct = default);
}