using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;

public interface IHotelRepository : IRepository<Hotel>
{
    Task<IEnumerable<Hotel>> GetActiveHotelsAsync();
    Task<bool> ExistsAsync(string name, string city, string country);
}