using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IPreferenceRepository : IRepository<Preferences>
{
    Task<Preferences> GetPreferenceByIdAsync(int id);
    Task<IEnumerable<Preferences>> GetAllPreferencesAsync();
}