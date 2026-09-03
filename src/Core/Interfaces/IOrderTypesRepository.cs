using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IOrderTypesRepository : IRepository<OrderType>
{
    Task<IEnumerable<OrderType>> GetOrderTypesAsync();
    Task<OrderType> GetOrderTypeByIdAsync(int orderTypeId);
}