using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;

public class RoomTypeRepository : GenericRepository<RoomType>, IRoomTypeRepository
{
    public RoomTypeRepository(HotelDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
    {
        return await _dbSet.ToListAsync();
    }
}