using System.ComponentModel.DataAnnotations;

namespace HotelManagementService.Application.DTOs;

//Restaurant Table DTOs
public record CreateRestaurantTableDto(
    [Required] int HotelId,
    [Required] string TableNumber,
    [Required] int Capacity
);

public record UpdateRestaurantTableDto(
    string? TableNumber,
    int? Capacity,
    string? Status
);

public record RestaurantTableDto(
    int Id,
    int HotelId,
    string TableNumber,
    int Capacity,
    string Status,
    string? HotelName = null
);

// Menu Item DTOs
public record CreateMenuItemDto(
    [Required] int HotelId,
    [Required] string Name,
    string? Description,
    [Required] decimal Price,
    [Required] string Category
);

public record UpdateMenuItemDto(
    string? Name,
    string? Description,
    decimal? Price,
    string? Category,
    bool? IsAvailable
);

public record MenuItemDto(
    int Id,
    int HotelId,
    string Name,
    string? Description,
    decimal Price,
    string Category,
    bool IsAvailable,
    string? HotelName = null
);

// Restaurant Order DTOs
public class CreateRestaurantOrderDto
{
    [Required] public int TableId {get;set;}
    public int? GuestId {get; set;}
    [Required] public int OrderTypeId {get; set;}
    [Required] public List<CreateOrderItemDto> Items {get; set;} = new ();
    public string? SpecialInstructions {get; set;}
}


public record CreateOrderItemDto(
    [Required] int MenuItemId,
    [Required] int Quantity,
    string? Notes
);

public record UpdateRestaurantOrderDto(
    string? Status,
    string? SpecialInstructions
);

public record RestaurantOrderDto(
    int Id,
    int TableId,
    int? GuestId,
    string Status,
    decimal TotalAmount,
    string OrderNumber,
    string OrderType,
    string? SpecialInstructions,
    DateTime CreatedAt,
    RestaurantTableDto? Table = null,
    GuestDto? Guest = null,
    List<OrderItemDto>? Items = null
);

// Order Item DTOs
public record UpdateOrderItemDto(
    int? Quantity,
    string? Notes
);

public record OrderItemDto(
    int Id,
    int OrderId,
    int MenuItemId,
    int Quantity,
    decimal Price,
    string? Notes,
    MenuItemDto? MenuItem = null
);
/*
// Hotel DTOs (referenced in the document but not defined)
public record CreateHotelDto(
    [Required] string Name,
    [Required] string Address
);

public record UpdateHotelDto(
    string? Name,
    string? Address
);

public record HotelDto(
    int Id,
    string Name,
    string Address,
    DateTime CreatedAt
);*/

// User DTOs (referenced in the document but not defined)
/*public record CreateUserDto(
    [Required] string Name,
    [Required] string Email,
    [Required] string Password,
    [Required] string Role
);*/

