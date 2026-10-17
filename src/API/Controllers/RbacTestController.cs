using HotelmanagementService.API.Authorization;
using HotelManagementService.Core.Constants;
using Microsoft.AspNetCore.Mvc;

namespace HotelmanagementService.API.Controllers;
[ApiController]
[Route("api/rbac-test")]
public class RbacTestController : ControllerBase
{
    [HttpGet("view")]
    [HasPermission(RbacPermissions.UsersView)]
    public IActionResult View() => Ok("Users.View granted");

    [HttpGet("delete")]
    [HasPermission(RbacPermissions.UsersDelete)]
    public IActionResult Delete() => Ok("Users.Delete granted");
}