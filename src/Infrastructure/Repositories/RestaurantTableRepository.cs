using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
// In RestaurantTableRepository implementation
public class RestaurantTableRepository : GenericRepository<RestaurantTable>, IRestaurantTableRepository
{
    public RestaurantTableRepository(HotelDbContext context) : base(context){}

    public async Task<IEnumerable<RestaurantTable>> GetAllWithDetailsAsync()
    {
        return await _context.RestaurantTables
            .Include(rt => rt.Hotel)
            .ToListAsync();
    }

    public async Task<RestaurantTable> GetByIdWithDetailsAsync(int id)
    {
        return await _context.RestaurantTables
            .Include(rt => rt.Hotel)
            .FirstOrDefaultAsync(rt => rt.Id == id);
    }
}