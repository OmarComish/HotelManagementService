using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var hotels = await _hotelService.GetAllHotelsAsync();
        return Ok(hotels);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var hotel = await _hotelService.GetHotelByIdAsync(id);
        if (hotel == null) return NotFound();
        return Ok(hotel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateHotelDto createDto)
    {
        try
        {
            var created = await _hotelService.CreateHotelAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateHotelDto updateDto)
    {
        var updated = await _hotelService.UpdateHotelAsync(id, updateDto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _hotelService.DeleteHotelAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}