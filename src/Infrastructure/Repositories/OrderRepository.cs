using HotelFlowAPI.Core.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
public class OrderRepository : GenericRepository<OrderItem>, IOrderRepository
{
    public OrderRepository(HotelDbContext context) : base(context) { }

    public async Task<IEnumerable<OrderItem>> GetOrdersAsync()
    {
        return await _context.OrderItems.ToListAsync();
    }
}