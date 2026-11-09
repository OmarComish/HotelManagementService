using HotelmanagementService.API.Authorization;
using HotelmanagementService.API.Filters;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Exceptions;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Constants;
using Microsoft.AspNetCore.Mvc;


namespace HotelmanagementService.API.Controllers;

[ApiController]
[Route("api/settings")]
[TypeFilter(typeof(RbacExceptionFilter))]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settings;

    public SettingsController(ISettingsService settings)
    {
        _settings = settings;
    }

    private string ActorUsername =>
        User.FindFirst("username")?.Value
        ?? throw new RbacException(401, "Unauthorized");

    [HttpGet]
    [HasPermission(RbacPermissions.SettingsView)]
    public async Task<IActionResult> Get(CancellationToken ct) =>
        Ok(await _settings.GetSettingsAsync(ct));

    [HttpPut("general")]
    [HasPermission(RbacPermissions.SettingsEdit)]
    public async Task<IActionResult> UpdateGeneral([FromBody] GeneralSettingsDto input, CancellationToken ct) =>
        Ok(await _settings.UpdateGeneralAsync(ActorUsername, input, ct));

    [HttpPut("security")]
    [HasPermission(RbacPermissions.SettingsEdit)]
    public async Task<IActionResult> UpdateSecurity([FromBody] SecuritySettingsDto input, CancellationToken ct) =>
        Ok(await _settings.UpdateSecurityAsync(ActorUsername, input, ct));
}