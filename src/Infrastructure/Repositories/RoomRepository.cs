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
        var rooms = await _context.Rooms
            .Include(r => r.RoomType)
            .ThenInclude(rt => rt.Amenities)
            .ThenInclude(a => a.Amenities)
            .Include(r => r.Hotel)
            .ToListAsync();

            // DEBUG: Check before mapping
        foreach (var room in rooms)
        {
            var amenityCount = room.RoomType?.Amenities?.Count ?? 0;
            var nestedCount = room.RoomType?.Amenities?.SelectMany(a => new[] { a.Amenities }).Count() ?? 0;
            Console.WriteLine($"Room {room.RoomNumber}: {amenityCount} join records, {nestedCount} amenities loaded");
        }
        return rooms;
    }

    public async Task<Room> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Rooms
            .Include(r => r.RoomType)
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}
