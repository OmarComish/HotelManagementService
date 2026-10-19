using HotelmanagementService.API.Authorization;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Constants;
using HotelManagementService.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelmanagementService.API.Controllers;
[ApiController]
[Route("api/audit-logs")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditService _audit;

    public AuditLogsController(IAuditService audit)
    {
        _audit = audit;
    }

    [HttpGet]
    [HasPermission(RbacPermissions.AuditLogsView)]
    public async Task<IActionResult> GetAll([FromQuery] AuditLogQuery query, CancellationToken ct) =>
        Ok(await _audit.GetLogsAsync(query, ct));
}