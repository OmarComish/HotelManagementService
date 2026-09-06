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
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _restaurantService.GetAllOrdersAsync();
        return Ok(orders);
    }
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        var order = await _restaurantService.GetOrderByIdAsync(orderId);
        if(order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }
}