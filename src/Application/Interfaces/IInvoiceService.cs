
using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface IInvoiceService
{
    Task<ResponseDto> GenerateCheckInInvoiceAsync(int reservationId);
    Task<ResponseDto> GenerateCheckOutInvoiceAsync(int reservationId);
    Task<ResponseDto> AddInvoiceLineItemAsync(int reservationId, LineItemsDto dto);
    Task<IEnumerable<ReadInvoiceDto>> GetInvoicesAsync();
    Task<ResponseDto> SettleInvoiceAsync(int reservationId);
    Task<ResponseDto> MarkInvoiceAsPaidAsync(MarkInvoiceAsPaidDto dto);
    Task<ResponseDto> SendInvoiceAsync(string invoiceNumber);
    Task<ResponseDto> DeleteInvoiceAsync(string invoiceNumber);
    Task<ResponseDto> UpdateInvoiceLineItem(string invoiceNumber, UpdateInvoiceLineItemsDto dto);
}