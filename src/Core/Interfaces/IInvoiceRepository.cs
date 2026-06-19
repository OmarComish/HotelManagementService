using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelFlowAPI.Core.Interfaces;
public interface IInvoiceRepository :IRepository<Invoice>
{
    Task<Invoice> GetByReservationAsync(int reservationId);
}