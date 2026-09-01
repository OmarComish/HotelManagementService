using HotelManagementService.Core.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;

public class PreferenceRepository : GenericRepository<Preferences>, IPreferenceRepository
{
    public PreferenceRepository(HotelDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Preferences>> GetAllPreferencesAsync()
    {
        return await _context.Preferences.ToListAsync();
    }

    public async Task<Preferences> GetPreferenceByIdAsync(int id)
    {
        return await _context.Preferences.FirstOrDefaultAsync(p => p.Id == id);
    }
}