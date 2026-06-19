using System.Collections;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelFlowAPI.Core.Interfaces;
public interface IRestaurantRepository :IRepository<RestaurantOrder>
{
    Task<IEnumerable<RestaurantOrder>> GetByReservationAsync(int guestId);
}