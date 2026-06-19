using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;

public interface IReservationRepository: IRepository<Reservation>
{
   Task<Reservation?> GetWithRoomDetailsByIdAsync(int id);
   Task<IEnumerable<Reservation?>> GetWithRoomDetailsAsync();

}