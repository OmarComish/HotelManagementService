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

}

