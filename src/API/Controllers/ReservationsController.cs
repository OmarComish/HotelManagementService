using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelFlowAPI.API.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class ReservationsController: ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IInvoiceService _invoiceService; 
    public ReservationsController(IReservationService reservationService, IInvoiceService invoiceService)
    {
        _reservationService = reservationService;
        _invoiceService = invoiceService;
    }
    [HttpPost]
    public async Task<ActionResult<ResponseDto>> CreateReservation(CreateReservationDto createReservationDto)
    {
        var response = new ResponseDto { Status = "error", Message = BadRequest().ToString() };
        if (createReservationDto != null)
        {
            Console.WriteLine($"{createReservationDto}");
            response = await _reservationService.CreateReservation(createReservationDto);
        }
        return Ok(response);
    }
    [HttpGet]
    public async Task<ActionResult<ReservationDto>> GetReservations()
    {
        var response = await _reservationService.GetAllReservations();
        return Ok(response);
    }
    [HttpPut]
    public async Task<ActionResult> UpdateReservation(UpdateReservationDto updatereservationDto)
    {
        var response = new ResponseDto{Status ="error", Message = BadRequest("Could not save the changes").ToString()};
        if(updatereservationDto!= null)
        {
            response.Payload = await _reservationService.UpdateReservationAsync(updatereservationDto);
        }
        return Ok(response);
    }
    [HttpPut]
    public async Task<ActionResult> CheckIn(CheckInDto dto)
    {
         var response = new ResponseDto{Status ="error", Message = BadRequest("Check-in failed.").ToString()};
         if(dto!= null)
        {
            //1. start by updating the reservation
            response = await _reservationService.CheckIn(dto);

            //2. if successful -> step 3, else step 4
            if(response.Status == "success")
            {
                //3. create checkin invoice
                response = await _invoiceService.GenerateCheckInInvoiceAsync(dto.ReservationId);
            }
            
           
        }
         //4. render results
        return Ok(response);
    }
}