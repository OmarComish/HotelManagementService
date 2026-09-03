using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
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
    public async Task<ResponseDto> AddOrderAsync(int hotelId, CreateRestaurantOrderDto dto)
    {
        
        try
        {
            //1. Validate table belongs to hotel
            var table = await _unitOfWork.RestaurantTables.GetByIdWithDetailsAsync(dto.TableId);
            if (table == null )
            {
                return new ResponseDto { Status = "error", Message = $"Table with ID {dto.TableId} not found in this hotel."};
            }

            //2. Validate Guest belongs to hotel
            if(dto.GuestId.HasValue)
            {
                var guest = await _unitOfWork.GuestPreferences.GetByIdWithDetailsAsync(dto.GuestId.Value);//(g=>g.Id==dto.GuestId.Value);//GetByIdWithDetailsAsync(dto.GuestId.Value);
                if (guest == null )//|| guest.HotelId != hotelId || guest.Status != "Approved")
                {
                    return new ResponseDto { Status = "error", 
                    Message = $"Guest with ID {dto.GuestId} is not approved or does not belong to this hotel."};
                }
            }
            //3. Validate menuitem exists and belongs to hotel
            var menuItemIds = dto.Items.Select(i => i.MenuItemId).ToList();
            var menuItems = (await _unitOfWork.MenuItems.
               FindAsync(mi => menuItemIds.Contains(mi.Id) && mi.HotelId == hotelId)).ToDictionary(m=>m.Id);

            var missingIds = menuItemIds.Except(menuItems.Keys).ToList();
            if(missingIds.Any())
            {
                return new ResponseDto { Status = "error", 
                Message = $"Menu items with IDs {string.Join(", ", missingIds)} not found in this hotel."};
            }

            var unavailableItems = menuItems.Values.Where(mi => !mi.IsAvailable).Select(mi => mi.Name).ToList();
            
            if(unavailableItems.Any())
            {
                return new ResponseDto { Status = "error", 
                Message = $"Menu items {string.Join(", ", unavailableItems)} are currently not available."};
            }

            //4. Build orderItems and Calculate total amount
            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach(var itemDto in dto.Items)
            {
                var menuItem = menuItems[itemDto.MenuItemId];
                orderItems.Add(new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    Quantity = itemDto.Quantity,
                    Price = menuItem.Price,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                });
                totalAmount += menuItem.Price * itemDto.Quantity;
            }

            //5. Create and Save the order
            var order = _mapper.Map<RestaurantOrder>(dto);
            order.TotalAmount = totalAmount;
            order.Status = "Available";
            order.Items = orderItems;
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            order.CreatedBy = "System"; // Replace with actual user info if available 

            await _unitOfWork.RestaurantOrders.AddAsync(order); 

            //6. Return mapped to DTO
            var orderDto = _mapper.Map<RestaurantOrderDto>(order);
            return new ResponseDto { Status = "success", Message = "Restaurant order created successfully.", Payload = orderDto };
        }
        catch (Exception ex)
        {
            return new ResponseDto 
            { 
                Status = "error", 
                Message = "An unexpected error occurred while creating the restaurant order: " + ex.Message
            };
        }
    }
}