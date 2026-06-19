using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

public interface IGuestPreferencesRepository: IRepository<GuestPreferences>
{
    Task<IEnumerable<GuestPreferences>> GetAllWithDetailsAsync();
    Task<IEnumerable<GuestPreferences>> GetByIdWithDetailsAsync(int id);
}