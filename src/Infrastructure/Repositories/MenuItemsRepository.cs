using HotelFlowAPI.Core.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
public class MenuItemsRepository : GenericRepository<MenuItem>, IMenuItemsRepository
{
    public MenuItemsRepository(HotelDbContext context) : base(context) { }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsByHotelIdAsync(int hotelId)
    {
        return await _context.MenuItems
            .Where(m => m.HotelId == hotelId)
            .ToListAsync();
    }
}