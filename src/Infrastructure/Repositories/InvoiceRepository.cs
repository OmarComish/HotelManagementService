using HotelFlowAPI.Core.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;

public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(HotelDbContext context): base(context){}
    public async Task<Invoice> GetByReservationAsync(int reservationId)
    {
        return await _context.Invoices.FirstOrDefaultAsync(i =>i.ReservationId == reservationId);
    }
    public async Task<IEnumerable<Invoice>> GetInvoicesAsync()
    {
        var invoice = await _context.Invoices
        .AsNoTracking()
        .AsSplitQuery()
        .Include(i => i.LineItems)
        .Include(i => i.Reservation)
            .ThenInclude(r => r.Guest)
        .Include(i => i.Reservation)
            .ThenInclude(r => r.Room)
            .Where(i => i.Status != InvoiceStatus.voided)
        .ToListAsync(); 

        return invoice;
    }
    public async Task<IEnumerable<Invoice>> GetInvoicesPendingCheckOutAsync()
    {
        var today = DateTime.UtcNow.Date;
        var tommorrow = today.AddDays(1);

        var invoice = await _context.Invoices
            .AsNoTracking()
            .AsSplitQuery()
            .Include(i => i.LineItems)
            .Include(i => i.Reservation)
                .ThenInclude(r => r.Guest)
            .Include(i => i.Reservation)
                .ThenInclude(r => r.Room)
            .Where(i => i.Reservation.CheckOut >= today 
            && i.Reservation.CheckOut < tommorrow && i.Reservation.Status == ReservationStatuses.CheckedIn) // Filter for invoices with reservations that are checked in and not yet checked out
            .ToListAsync(); 

        return invoice;
    }
    public async Task<Invoice> GetInvoiceByNumberAsync(string invoiceNumber)
    {
        return await _context.Invoices.FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
    }

}

