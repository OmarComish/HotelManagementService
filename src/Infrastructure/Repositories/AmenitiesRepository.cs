using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
public class AmenitiesRepository : GenericRepository<Amenities>, IAmenitiesRepository
{
    public AmenitiesRepository(HotelDbContext context) : base(context)
    {
    }
    public async Task<Amenities> GetPreferenceByIdAsync(int id)
    {
        return await _context.Amenities.FindAsync(id);
    }

    public async Task<IEnumerable<Amenities>> GetAllPreferencesAsync()
    {
        return await _context.Amenities.ToListAsync();
    }
}