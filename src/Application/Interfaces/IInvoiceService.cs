
using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface IInvoiceService
{
    Task<ResponseDto> GenerateCheckInInvoiceAsync(int reservationId);
    Task<ResponseDto> GenerateCheckOutInvoiceAsync(int reservationId);
    Task<ResponseDto> AddInvoiceLineItemAsync(int reservationId, LineItemsDto dto);
}