using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementService.API.Controllers;
[ApiController]
[Route("api/[controller]")]

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
    [HttpGet("all")]
    public async Task<ActionResult> Get()
    {
        var response = await _invoiceservice.GetInvoicesAsync();
        return Ok(response);
    }
   [HttpPut("markpaid")]
    public async Task<ActionResult<ResponseDto>> MarkAsPaid(MarkInvoiceAsPaidDto dto)
    {
        var response = new ResponseDto { Status = "error", Message = BadRequest().ToString() };
        if(!string.IsNullOrEmpty(dto.InvoiceNumber))
        {
            response = await _invoiceservice.MarkInvoiceAsPaidAsync(dto);
        }
        return Ok(response);
    }
    [HttpPost("send/{invoiceNumber}")]
    public async Task<ActionResult<ResponseDto>> SendInvoice(string invoiceNumber)
    {
        var response = new ResponseDto { Status = "error", Message = BadRequest().ToString() };
        if(!string.IsNullOrEmpty(invoiceNumber))
        {
            response = await _invoiceservice.SendInvoiceAsync(invoiceNumber);
        }
        return Ok(response);
    }
    [HttpDelete("{invoiceNumber}")]
    public async Task<ActionResult<ResponseDto>> DeleteInvoice(string invoiceNumber)
    {
        var response = new ResponseDto { Status = "error", Message = BadRequest().ToString() };
        if(!string.IsNullOrEmpty(invoiceNumber))
        {
            response = await _invoiceservice.DeleteInvoiceAsync(invoiceNumber);
        }
        return Ok(response);
    }
}