using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;

public class InvoiceLineItemRepository : GenericRepository<InvoiceLineItem>, IInvoiceLineItemRepository
{
    public InvoiceLineItemRepository(HotelDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<InvoiceLineItem>> GetInvoiceLineItemsAsync()
    {
        return await _dbSet.ToListAsync();
    }
}

