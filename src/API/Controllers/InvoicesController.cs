using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementService.API.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]

public class InvoicesController: ControllerBase
{
    private readonly IInvoiceService _invoiceservice;
    public InvoicesController(IInvoiceService invoiceService){_invoiceservice = invoiceService;}
    [HttpPost("{reservationId}")]
    public async Task<ActionResult<ResponseDto>> CreateLineItem(int reservationId, [FromBody] LineItemsDto lineItemdto)
    {
        var response = new ResponseDto { Status = "error", Message = BadRequest().ToString() };
        if(lineItemdto!=null)
        {
            response = await _invoiceservice.AddInvoiceLineItemAsync(reservationId, lineItemdto);
        }
        return response;
    }

}