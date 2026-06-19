using HotelFlowAPI.Core.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace HotelManagementService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly HotelDbContext _context;
    private IDbContextTransaction? _transaction;

    public IHotelRepository Hotels { get; }
    public IReservationRepository Reservations {get;}
    public IRoomRepository Rooms {get;}
    public IRoomTypeRepository RoomTypes {get;}
    public IInvoiceRepository Invoices {get;}
    public IRestaurantRepository Restaurants {get;}
    public IInvoiceLineItemRepository InvoiceLineItems {get;}
    public IGuestRepository Guests {get;}
    public IGuestPreferencesRepository GuestPreferences {get;}
    public ITaskTypeRepository TaskTypes {get;}
    public ITaskRepository TaskItems {get;}
    public IUsersRepository Users {get;}
    public IEmployeeRepository Employees {get;}

    public UnitOfWork(HotelDbContext context)
    {
        _context = context;
        
        Hotels = new HotelRepository(_context);
        Reservations = new ReservationRepository(_context);
        Rooms = new RoomRepository(_context);
        RoomTypes = new RoomTypeRepository(_context);
        Invoices = new InvoiceRepository(_context);
        Restaurants = new RestaurantRepository(_context);
        InvoiceLineItems = new InvoiceLineItemRepository(_context);
        Guests = new GuestRepository(_context);
        GuestPreferences = new GuestPreferencesRepository(_context);
        TaskTypes = new TaskTypeRepository(_context);
        TaskItems = new TaskRepository(_context);
        Users = new UserRepository(_context);
        Employees = new EmployeeRepository(_context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
            await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
            await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}