using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelmanagementService.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AmenitiesController : ControllerBase
{
    private readonly IAmenitiesService _amenitiesService;

    public AmenitiesController(IAmenitiesService amenitiesService)
    {
        _amenitiesService = amenitiesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAmenities()
    {
        var amenities = await _amenitiesService.GetAllAmenitiesAsync();
        return Ok(amenities);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAmenityById(int id)
    {
        var amenity = await _amenitiesService.GetAmenityByIdAsync(id);
        if (amenity == null)
        {
            return NotFound();
        }
        return Ok(amenity);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAmenity([FromBody] CreateAmenitiesDto createAmenitiesDto)
    {
        var response = await _amenitiesService.CreateAmenity(createAmenitiesDto);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAmenity(int id, [FromBody] UpdateAmenitiesDto updateAmenitiesDto)
    {
        var updatedAmenity = await _amenitiesService.UpdateAmenitiesAsync(id, updateAmenitiesDto);
        return Ok(updatedAmenity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAmenity(int id)
    {
        var result = await _amenitiesService.DeleteAmenityAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}