using HotelFlowAPI.Core.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
public class RestaurantRepository: GenericRepository<RestaurantOrder>, IRestaurantRepository
{
    public RestaurantRepository(HotelDbContext context): base(context){}
    public async Task<IEnumerable<RestaurantOrder>> GetByReservationAsync(int reservationId)
    {
        return await _context.RestaurantOrders
        .Include(o =>o.Table)
        .Include(o => o.Items)
        .Where(o =>o.ReservationId == reservationId)
        .ToListAsync();
    }
}
