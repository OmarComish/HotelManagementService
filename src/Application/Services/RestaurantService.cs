using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;
public class RestaurantService : IRestaurantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RestaurantService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> AddMenuItemAsync(int hotelId, CreateMenuItemDto dto)
    {
        // Implementation for adding menu item
        throw new NotImplementedException();
    }

    public async Task<ResponseDto> UpdateMenuItemAsync(int menuItemId, CreateMenuItemDto dto)
    {
        // Implementation for updating menu item
        throw new NotImplementedException();
    }

    public async Task<ResponseDto> DeleteMenuItemAsync(int menuItemId)
    {
        // Implementation for deleting menu item
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<MenuItemDto>> GetMenuItemsByHotelIdAsync(int hotelId)
    {
        var results = await _unitOfWork.MenuItems.GetAllAsync();
        
        Console.WriteLine($"Total menu items for hotel {hotelId}: {results.Count()}");
        if (results == null)
        {
            throw new KeyNotFoundException($"No restaurant found for hotel {hotelId}");
        }

        if (results == null)
        {
            return Enumerable.Empty<MenuItemDto>();
        }
        
        return _mapper.Map<IEnumerable<MenuItemDto>>(results);
    }
}