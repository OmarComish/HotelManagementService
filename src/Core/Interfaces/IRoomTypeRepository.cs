
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IRoomTypeRepository: IRepository<RoomType>
{
    Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();
}