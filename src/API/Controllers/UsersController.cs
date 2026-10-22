using HotelmanagementService.API.Authorization;
using HotelmanagementService.API.Filters;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Exceptions;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Constants;
using HotelManagementService.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementService.API.Controllers;

[ApiController]
[Route("api/users")]
[TypeFilter(typeof(RbacExceptionFilter))]
public class UsersController : ControllerBase
{
    private readonly IUserManagementService _users;

    public UsersController(IUserManagementService users)
    {
        _users = users;
    }

    private string ActorUsername =>
        User.FindFirst("username")?.Value
        ?? throw new RbacException(401, "Unauthorized");

    [HttpGet]
    [HasPermission(RbacPermissions.UsersView)]
    public async Task<IActionResult> GetAll([FromQuery] UserQuery query, CancellationToken ct) =>
        Ok(await _users.GetUsersAsync(query, ct));

    [HttpGet("{id:int}")]
    [HasPermission(RbacPermissions.UsersView)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var user = await _users.GetUserAsync(id, ct);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPut("{id:int}/role")]
    [HasPermission(RbacPermissions.UsersChangeRole)]
    public async Task<IActionResult> ChangeRole(int id, [FromBody] ChangeRoleRequest request, CancellationToken ct) =>
        Ok(await _users.ChangeRoleAsync(ActorUsername, id, request.RoleKey, ct));

    [HttpPut("{id:int}/status")]
    [Authorize]
    public async Task<IActionResult> SetStatus(int id, [FromBody] SetStatusRequest request, CancellationToken ct) =>
        Ok(await _users.SetStatusAsync(ActorUsername, id, request.Status, ct));

    [HttpDelete("{id:int}")]
    [HasPermission(RbacPermissions.UsersDelete)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _users.DeleteUserAsync(ActorUsername, id, ct);
        return NoContent();
    }
    
    [HttpPut("{id:int}")]
    [HasPermission(RbacPermissions.UsersEdit)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request, CancellationToken ct) =>
        Ok(await _users.UpdateUserAsync(ActorUsername, id, request, ct));
    
}