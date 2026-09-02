using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IOrderRepository : IRepository<OrderItem>
{
    Task<IEnumerable<OrderItem>> GetOrdersAsync();
}