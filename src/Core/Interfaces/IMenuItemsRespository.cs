using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelFlowAPI.Core.Interfaces;
public interface IMenuItemsRepository : IRepository<MenuItem>
{
    Task<IEnumerable<MenuItem>> GetMenuItemsByHotelIdAsync(int hotelId);
}