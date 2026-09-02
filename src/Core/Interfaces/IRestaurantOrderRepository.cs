using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IRestaurantOrderRepository : IRepository<RestaurantOrder>
{
    Task<IEnumerable<RestaurantOrder>> GetRestaurantOrdersAsync();
    Task<RestaurantOrder> GetRestaurantOrderByIdAsync(int orderId);
}