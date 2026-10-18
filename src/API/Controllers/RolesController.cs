using HotelmanagementService.API.Authorization;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Constants;
using Microsoft.AspNetCore.Mvc;

namespace HotelmanagementService.API.Controllers;

[ApiController]
[Route("api/roles")]
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
}