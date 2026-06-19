using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
public class ReservationRepository: GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(HotelDbContext context) : base(context){}
   
    public async Task<Reservation?> GetWithRoomDetailsByIdAsync(int id)
    {
        return await ReservationWithDetails.FirstOrDefaultAsync(r =>r.Id == id);
    }
    public async Task<IEnumerable<Reservation?>> GetWithRoomDetailsAsync() 
    {
        return await ReservationWithDetails.ToListAsync();
    }
    private IQueryable<Reservation> ReservationWithDetails =>
        _context.Reservations
        .Include(r =>r.Room)
        .ThenInclude(r =>r.RoomType)
            .ThenInclude(rt => rt.Amenities)
        .Include(r =>r.Room)
        .ThenInclude(r =>r.Hotel);
    
}
