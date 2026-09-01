using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelFlowAPI.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class GuestPreferenceController : ControllerBase
{
    private readonly ILogger<GuestsController> _logger;
    private readonly IGuestPreferenceService _guestPreferenceService;
    public GuestPreferenceController(ILogger<GuestsController> logger, IGuestPreferenceService guestPreferenceService)
    {
        _logger = logger;
        _guestPreferenceService = guestPreferenceService;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGuestPreference(int id)
    {
        var guestPreference = await _guestPreferenceService.GetGuestPreferenceByIdAsync(id);
        if (guestPreference == null)
        {
            return NotFound();
        }
        return Ok(guestPreference);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllGuestPreferences()
    {
        var guestPreferences = await _guestPreferenceService.GetAllGuestPreferencesAsync();
        return Ok(guestPreferences);
    }
}