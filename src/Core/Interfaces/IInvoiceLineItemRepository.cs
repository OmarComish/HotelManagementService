using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;

public interface IInvoiceLineItemRepository : IRepository<InvoiceLineItem>
{
    Task<IEnumerable<InvoiceLineItem>> GetInvoiceLineItemsAsync();
}