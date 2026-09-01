using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreferencesController : ControllerBase
{
    private readonly IPreferenceService _preferenceService;

    public PreferencesController(IPreferenceService preferenceService)
    {
        _preferenceService = preferenceService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePreference([FromBody] CreatePreferenceDto createPreferenceDto)
    {
        var preference = await _preferenceService.CreatePreferenceAsync(createPreferenceDto);
        return CreatedAtAction(nameof(GetPreferenceById), new { id = preference.Id }, preference);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPreferenceById(int id)
    {
        var preference = await _preferenceService.GetPreferenceByIdAsync(id);
        if (preference == null)
        {
            return NotFound();
        }
        return Ok(preference);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPreferences()
    {
        var preferences = await _preferenceService.GetAllPreferencesAsync();
        return Ok(preferences);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePreference(int id, [FromBody] UpdatePreferenceDto updatePreferenceDto)
    {
        var preference = await _preferenceService.UpdatePreferenceAsync(id, updatePreferenceDto);
        return Ok(preference);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePreference(int id)
    {
        var result = await _preferenceService.DeletePreferenceAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}