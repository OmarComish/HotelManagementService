using HotelmanagementService.API.Authorization;
using HotelmanagementService.API.Filters;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Exceptions;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Constants;
using Microsoft.AspNetCore.Mvc;

namespace HotelmanagementService.API.Controllers;

[ApiController]
[Route("api/roles")]
[TypeFilter(typeof(RbacExceptionFilter))]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    [HasPermission(RbacPermissions.RolesView)]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _roleService.GetRolesAsync(ct));

    [HttpGet("{id:int}")]
    [HasPermission(RbacPermissions.RolesView)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var role = await _roleService.GetRoleAsync(id, ct);
        return role is null ? NotFound() : Ok(role);
    }

    [HttpGet("permissions")]
    [HasPermission(RbacPermissions.RolesView)]
    public async Task<IActionResult> GetPermissions(CancellationToken ct) =>
        Ok(await _roleService.GetPermissionsAsync(ct));

    private string ActorUsername =>
       User.FindFirst("username")?.Value
       ?? throw new RbacException(401, "Unauthorized");

    [HttpPost]
    [HasPermission(RbacPermissions.RolesCreate)]
    public async Task<IActionResult> Create([FromBody] RoleInput input, CancellationToken ct)
    {
        var role = await _roleService.CreateRoleAsync(ActorUsername, input, ct);
        return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
    }

    [HttpPut("{id:int}")]
    [HasPermission(RbacPermissions.RolesEdit)]
    public async Task<IActionResult> Update(int id, [FromBody] RoleInput input, CancellationToken ct) =>
        Ok(await _roleService.UpdateRoleAsync(ActorUsername, id, input, ct));

    
    [HttpDelete("{id:int}")]
    [HasPermission(RbacPermissions.RolesDelete)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _roleService.DeleteRoleAsync(id, ct);
        return NoContent();
    }
}