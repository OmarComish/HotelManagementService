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
        return await GetAllReservations(); //ReservationWithDetails.ToListAsync();
    }
    private IQueryable<Reservation> ReservationWithDetails =>
        _context.Reservations
        .Include(r =>r.Room)
        .ThenInclude(r =>r.RoomType)
            .ThenInclude(rt => rt.Amenities)
        .Include(r =>r.Room)
        .ThenInclude(r =>r.Hotel)
        .Include(r =>r.Guest);

    public async Task<Reservation> GetCurrentReservationByRoomNumberAsync(string roomNumber)
    {
        var response = await( from reservation in _context.Reservations
            join room in _context.Rooms on reservation.RoomId equals room.Id
            where room.RoomNumber == roomNumber //&& reservation.Status == ReservationStatuses.Reserved
            //&& room.Status == RecordStatus.Occupied
            select reservation).FirstOrDefaultAsync();

            Console.WriteLine($"RESPONSE FROM CURRENT RESERVATION BY ROOM NUMBER {response?.GuestId}");
        
        return response;
    }
    private async Task<IEnumerable<Reservation>> GetAllReservations()
    {
        var response = await _context.Reservations
            .Include(r =>r.Guest)
            .Include(r =>r.Room).ToListAsync();

        return response;   
    }
    
}
