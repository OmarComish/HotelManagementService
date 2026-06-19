using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
public class GuestRepository: GenericRepository<Guest>, IGuestRepository
{
    public GuestRepository(HotelDbContext context) : base(context){}
    public async Task<Guest> GuestWithPreferenceAsync(int id)
    {
        var guest = await _context.Guests.Include(g => g.GuestPreferences)
                   .ThenInclude(gp => gp.Preferences)
                   .Include(g => g.Bookings)
                   .ThenInclude(g => g.Room)
                   .FirstOrDefaultAsync(g => g.Id == id);
                   
        return guest ?? throw new KeyNotFoundException($"Guest with Id {id} not found.");
    }
    public async Task<IEnumerable<Guest>> AllGuestsWithPreferenceAsync()
    {
        return await _context.Guests.Include(g => g.GuestPreferences)
                     .ThenInclude(gp => gp.Preferences)
                     .Include(g => g.Bookings)
                     .ThenInclude(g => g.Room).ToListAsync();
    }
}