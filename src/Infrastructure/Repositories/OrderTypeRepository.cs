using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
public class OrderTypeRepository : GenericRepository<OrderType>, IOrderTypesRepository
{
    public OrderTypeRepository(HotelDbContext context) : base(context) { }

    public async Task<IEnumerable<OrderType>> GetOrderTypesAsync()
    {
        return await _context.OrderTypes.ToListAsync();
    }

    public async Task<OrderType> GetOrderTypeByIdAsync(int orderTypeId)
    {
        return await _context.OrderTypes.FindAsync(orderTypeId);
    }
}