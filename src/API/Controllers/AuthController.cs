using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementService.API.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class AuthController: ControllerBase
{
    private readonly IAuthenticationService _authservice;
    private readonly IPermissionService _permissionService;
    public AuthController(IAuthenticationService authservice, IPermissionService permissionService)
    {
        _authservice = authservice;
        _permissionService = permissionService;
    }
    [HttpPost("Signin")]
    public async Task<ActionResult<AuthResponseDto>> SignIn([FromBody] LogInRequestDto request)
    {
        var response = new AuthResponseDto{Status ="error", Message = BadRequest("Invalid login details").ToString()};
        if(request != null) 
        {
             response = await _authservice.AuthenticateUser(request);
        }
       
        return Ok(response);
    }
    [Authorize(Policy = "ApiScope")]
    [HttpPost("registerUser")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
    {
        var response = new ResponseDto {Status="error", Message=$"{BadRequest("Failed to register user")}"};
        if(!ModelState.IsValid)
           return Ok(response);
        response = await _authservice.RegisterUser(dto);
        return Ok(response);

    }
    [Authorize]
    [HttpGet("Permissions")]
    public async Task<IActionResult> GetMyPermissions(CancellationToken ct)
    {
        var username = User.FindFirst("username")?.Value;

        if (string.IsNullOrWhiteSpace(username))
        return Unauthorized();

        var access = await _permissionService.GetAccessAsync(username, ct);
        if(access is null)
          return Unauthorized();

        return Ok(new
        {
           username = access.Username,
           role = access.RoleKey,
           permission = access.Permissions 
        });
    }
}