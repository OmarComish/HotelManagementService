using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelFlowAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GuestsController : ControllerBase
{
    private readonly IGuestService _guestService;
    private readonly ILogger<GuestsController> _logger;

    public GuestsController(IGuestService guestService, ILogger<GuestsController> logger)
    {
        _guestService = guestService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new guest
    /// </summary>
    /// <param name="createGuestDto">Guest details</param>
    /// <returns>Created guest</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GuestDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<GuestDto>> CreateGuest([FromBody] CreateGuestDto createGuestDto)
    {
        try
        {
            Console.WriteLine("logging guest dto");
            Console.WriteLine("\n");
            Console.WriteLine(createGuestDto.PreferenceIds);
            var guest = await _guestService.CreateGuestAsync(createGuestDto);
            return CreatedAtAction(nameof(GetGuest), new { id = guest.Id }, guest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating guest");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Gets a guest by ID
    /// </summary>
    /// <param name="id">Guest ID</param>
    /// <returns>Guest details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GuestDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GuestDto>> GetGuest(int id)
    {
        var guest = await _guestService.GetGuestByIdAsync(id);
        
        if (guest == null)
            return NotFound($"Guest with ID {id} not found");

        return Ok(guest);
    }

    /// <summary>
    /// Gets all guests
    /// </summary>
    /// <returns>List of guests</returns>
    [HttpGet]
    //[ProducesResponseType(typeof(IEnumerable<GuestDto>), 200)]
    public async Task<ActionResult<IEnumerable<GuestDto>>> GetAllGuests()
    {
        var guests = await _guestService.GetAllGuestsAsync();
        return Ok(guests);
    }

    /// <summary>
    /// Updates a guest
    /// </summary>
    /// <param name="id">Guest ID</param>
    /// <param name="updateGuestDto">Updated guest details</param>
    /// <returns>Updated guest</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GuestDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GuestDto>> UpdateGuest(int id, [FromBody] UpdateGuestDto updateGuestDto)
    {
        try
        {
            var guest = await _guestService.UpdateGuestAsync(id, updateGuestDto);
            return Ok(guest);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating guest {GuestId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Deletes a guest
    /// </summary>
    /// <param name="id">Guest ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> DeleteGuest(int id)
    {
        try
        {
            var result = await _guestService.DeleteGuestAsync(id);
            
            if (!result)
                return NotFound($"Guest with ID {id} not found");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting guest {GuestId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}

