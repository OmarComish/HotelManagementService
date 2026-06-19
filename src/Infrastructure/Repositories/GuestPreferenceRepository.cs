using HotelManagementService.Core.Entities;
using HotelManagementService.Infrastructure.Data;
using HotelManagementService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class GuestPreferencesRepository: GenericRepository<GuestPreferences>, IGuestPreferencesRepository
{
    public GuestPreferencesRepository(HotelDbContext context) :base(context){}

    public async Task<IEnumerable<GuestPreferences>> GetAllWithDetailsAsync()
    {
        return await _context.GuestPreferences
        .Include(p=>p.Preferences)
        .ToListAsync();
    }

    public async Task<IEnumerable<GuestPreferences>> GetByIdWithDetailsAsync(int id)
    {
        return await _context.GuestPreferences
        .Include(p =>p.Preferences)
        .Where(g=>g.GuestId == id)
        .ToListAsync();
    }
}
