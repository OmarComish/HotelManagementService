using HotelFlowAPI.Core.Interfaces;
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IHotelRepository Hotels { get; }
    IReservationRepository Reservations {get;}
    IRoomRepository Rooms {get;}
    IRoomTypeRepository RoomTypes {get;}
    IInvoiceRepository Invoices {get;}
    IRestaurantRepository Restaurants {get;}
    IInvoiceLineItemRepository InvoiceLineItems {get;}
    IGuestRepository Guests {get;}
    IGuestPreferencesRepository GuestPreferences {get;}
    IPreferenceRepository Preferences {get;}
    ITaskTypeRepository TaskTypes {get;}
    ITaskRepository TaskItems {get;}
    IUsersRepository Users {get;}
    IEmployeeRepository Employees {get;}
    IMenuItemsRepository MenuItems {get;}
    IAmenitiesRepository Amenities {get;}
    IOrderRepository Orders {get;}
    IRestaurantTableRepository RestaurantTables {get;}
    IRestaurantOrderRepository RestaurantOrders {get;}
    
    //Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}