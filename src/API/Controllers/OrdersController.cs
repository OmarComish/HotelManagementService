using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementService.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class OrdersController: ControllerBase
{
    private readonly IRestaurantService _restaurantService;
    public OrdersController(IRestaurantService restaurantService)
    {
        _restaurantService =  restaurantService;
    }
    [HttpPost]
    public async Task<IActionResult> AddOrder(CreateRestaurantOrderDto dto)
    {
        var response = new ResponseDto {Status ="error", 
        Message = BadRequest("Null or Invalid order details. Failed to add order").ToString()};
        if(dto!=null)
        {
            int hotelId = 1; //Hard coded value
            response = await _restaurantService.AddOrderAsync(hotelId,dto);
        }
        return Ok(response);
    }
}