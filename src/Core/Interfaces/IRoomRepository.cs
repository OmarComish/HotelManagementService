using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IRoomRepository: IRepository<Room>
{
     Task<IEnumerable<Room>> GetAllWithDetailsAsync();
     Task<Room> GetByIdWithDetailsAsync(int id);
}