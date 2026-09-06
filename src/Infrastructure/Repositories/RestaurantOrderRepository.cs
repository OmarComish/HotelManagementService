using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
public class RestaurantOrderRepository : GenericRepository<RestaurantOrder>, IRestaurantOrderRepository
{
    public RestaurantOrderRepository(HotelDbContext context) : base(context) { }

    public async Task<IEnumerable<RestaurantOrder>> GetRestaurantOrdersAsync()
    {
        return await _context.RestaurantOrders.Include(o => o.Table)
            .Include(o =>o.OrderType)
            .Include(o => o.Table.Hotel)
            .Include(o =>o.Guest)
            .Include(o => o.Items)
            .ThenInclude(i =>i.MenuItem)
            .ThenInclude(m => m.Hotel)
            .ToListAsync();
    }

    public async Task<RestaurantOrder> GetRestaurantOrderByIdAsync(int orderId)
    {
        return await _context.RestaurantOrders
            .Include(o => o.Table)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public Task<int> CountRestaurantOrdersAsync(int ordertypeId)
    {
        return _context.RestaurantOrders.CountAsync(o => o.OrderTypeId == ordertypeId);
    }
}