using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface IRestaurantService
{
    Task<ResponseDto> AddMenuItemAsync(int hotelId, CreateMenuItemDto dto);
    Task<ResponseDto> UpdateMenuItemAsync(int menuItemId, CreateMenuItemDto dto);
    Task<ResponseDto> DeleteMenuItemAsync(int menuItemId);
    Task<IEnumerable<MenuItemDto>> GetMenuItemsByHotelIdAsync(int hotelId);
    Task<ResponseDto> AddOrderAsync(int hotelId, CreateRestaurantOrderDto dto);
}