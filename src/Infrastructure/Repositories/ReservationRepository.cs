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
        var response = await _context.Reservations
            .Include(r =>r.Guest)
            .Include(r =>r.Room)
            .Where(r => r.Room.RoomNumber == roomNumber)
            .FirstOrDefaultAsync();
                    
        return response;
    }
    public async Task<IEnumerable<Reservation>> GetAllReservationsPendingCheckOutAsync()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var reservations = await _context.Reservations
            .AsNoTracking()
            .Where(r =>
                r.CheckOut >= today &&
                r.CheckOut < tomorrow)
            .Select(r => new Reservation
            {
                Id = r.Id,
                CheckIn = r.CheckIn,
                CheckOut = r.CheckOut,
                Status = r.Status
            })
            .ToListAsync();

        return reservations;
    }

    public async Task<Reservation?> GetActiveReservationByGuestAsync(int guestId)
    {
        return await _context.Reservations
            .Include(r => r.Room)
            .Include(r => r.Guest)
            .FirstOrDefaultAsync(r =>
                r.GuestId == guestId &&
                r.Status == ReservationStatuses.CheckedIn);
    }
    private async Task<IEnumerable<Reservation>> GetAllReservations()
    {
        var response = await _context.Reservations
            .Include(r =>r.Guest)
            .Include(r =>r.Room).ToListAsync();

        return response;   
    }
    
}
