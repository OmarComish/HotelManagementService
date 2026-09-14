using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IRoomRepository: IRepository<Room>
{
     Task<IEnumerable<Room>> GetAllWithDetailsAsync();
     Task<Room> GetByIdWithDetailsAsync(int id);
     Task<Room> GetByRoomTypeWithDetailsAsync(int HotelId, int roomTypeId, DateTime checkIn, DateTime checkOut);
}