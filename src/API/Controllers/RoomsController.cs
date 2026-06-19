using HotelManagementService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly ILogger<RoomsController> _logger;

    public RoomsController(IRoomService roomService, ILogger<RoomsController> logger)
    {
        _roomService = roomService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new room
    /// </summary>
    /// <param name="createRoomDto">Room details</param>
    /// <returns>Created room</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RoomDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<ActionResult<RoomDto>> CreateRoom([FromBody] CreateRoomDto createRoomDto)
    {
        try
        {
            
            var room = await _roomService.CreateRoomAsync(createRoomDto);
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating room");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Gets a room by ID
    /// </summary>
    /// <param name="id">Room ID</param>
    /// <returns>Room details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RoomDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<RoomDto>> GetRoom(int id)
    {
        var room = await _roomService.GetRoomByIdAsync(id);
        
        if (room == null)
            return NotFound($"Room with ID {id} not found");

        return Ok(room);
    }

    /// <summary>
    /// Gets all rooms
    /// </summary>
    /// <returns>List of rooms</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoomDto>), 200)]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRooms()
    {
        var rooms = await _roomService.GetAllRoomsAsync();
        return Ok(rooms);
    }

    /// <summary>
    /// Gets available rooms for specific dates
    /// </summary>
    /// <param name="hotelId">Hotel ID</param>
    /// <param name="checkIn">Check-in date</param>
    /// <param name="checkOut">Check-out date</param>
    /// <returns>List of available rooms</returns>
    [HttpGet("available")]
    [ProducesResponseType(typeof(IEnumerable<RoomDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAvailableRooms(
        [FromQuery] int hotelId, 
        [FromQuery] DateTime checkIn, 
        [FromQuery] DateTime checkOut)
    {
        if (checkIn >= checkOut)
            return BadRequest("Check-in date must be before check-out date");

        var rooms = await _roomService.GetAvailableRoomsAsync(hotelId, checkIn, checkOut);
        return Ok(rooms);
    }

    /// <summary>
    /// Updates a room
    /// </summary>
    /// <param name="id">Room ID</param>
    /// <param name="updateRoomDto">Updated room details</param>
    /// <returns>Updated room</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RoomDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<RoomDto>> UpdateRoom(int id, [FromBody] UpdateRoomDto updateRoomDto)
    {
        try
        {
            var room = await _roomService.UpdateRoomAsync(id, updateRoomDto);
            return Ok(room);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating room {RoomId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Deletes a room
    /// </summary>
    /// <param name="id">Room ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        try
        {
            var result = await _roomService.DeleteRoomAsync(id);
            
            if (!result)
                return NotFound($"Room with ID {id} not found");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting room {RoomId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}