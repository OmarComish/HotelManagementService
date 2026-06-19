using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
// In RoomRepository implementation
public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(HotelDbContext context) : base(context){}

    public async Task<IEnumerable<Room>> GetAllWithDetailsAsync()
    {
        return await _context.Rooms
            .Include(r => r.RoomType)
            .Include(r => r.Hotel)
            .ToListAsync();
    }

    public async Task<Room> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Rooms
            .Include(r => r.RoomType)
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}
