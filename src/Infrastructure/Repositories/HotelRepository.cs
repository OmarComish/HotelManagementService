using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;

public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    public HotelRepository(HotelDbContext context) : base(context) { }

    public async Task<IEnumerable<Hotel>> GetActiveHotelsAsync()
        => await _dbSet.Where(h => h.IsActive).ToListAsync();

    public async Task<bool> ExistsAsync(string name, string city, string country)
        => await _dbSet.AnyAsync(h => h.Name == name && h.City == city && h.Country == country);
}