using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IRestaurantTableRepository: IRepository<RestaurantTable>
{
     Task<IEnumerable<RestaurantTable>> GetAllWithDetailsAsync();
     Task<RestaurantTable> GetByIdWithDetailsAsync(int id);
}