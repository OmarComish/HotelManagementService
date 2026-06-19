using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface IGuestRepository: IRepository<Guest>
{
    Task<Guest> GuestWithPreferenceAsync(int id);
    Task<IEnumerable<Guest>> AllGuestsWithPreferenceAsync();
}