using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IAmenitiesRepository: IRepository<Amenities>
{
    Task<Amenities> GetPreferenceByIdAsync(int id);
    Task<IEnumerable<Amenities>> GetAllPreferencesAsync();
}