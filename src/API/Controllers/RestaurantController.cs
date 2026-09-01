using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementService.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    [HttpGet("{hotelId}/menu-items")]
    public async Task<IActionResult> GetMenuItemsByHotelId(int hotelId)
    {
        var menuItems = await _restaurantService.GetMenuItemsByHotelIdAsync(hotelId);
        return Ok(menuItems);
    }
}