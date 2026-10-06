using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementService.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReservationsController: ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IInvoiceService _invoiceService;
    private readonly IGuestService _guestService; 
    public ReservationsController(IReservationService reservationService, IInvoiceService invoiceService, IGuestService guestService)
    {
        _reservationService = reservationService;
        _invoiceService = invoiceService;
        _guestService = guestService;
    }
    [HttpPost]
    public async Task<ActionResult<ResponseDto>> CreateReservation(CreateReservationDto createReservationDto)
    {
        var response = new ResponseDto { Status = "error", Message = BadRequest().ToString() };
        Console.WriteLine($"{createReservationDto}");
        if (createReservationDto != null)
        {
            
            response = await _reservationService.CreateReservation(createReservationDto);
        }
        return Ok(response);
    }
    [HttpGet("all")]
    public async Task<ActionResult<ReservationDto>> GetReservations()
    {
        var response = await _reservationService.GetAllReservations();
        return Ok(response);
    }
    [HttpPut("update")]
    public async Task<ActionResult> UpdateReservation(UpdateReservationDto updatereservationDto)
    {
        var response = new ResponseDto{Status ="error", Message = BadRequest("Could not save the changes").ToString()};
        if(updatereservationDto!= null)
        {
            response.Payload = await _reservationService.UpdateReservationAsync(updatereservationDto);
            if(response.Payload != null)
            {
                response.Status = "success";
                response.Message = "Reservation updated successfully";
            }
        }
        return Ok(response);
    }
    [HttpPut("checkin")]
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
    [HttpPost("walkin-checkin")]
    public async Task<ActionResult<ResponseDto>> WalkinCheckin(CreateWalkInCheckinDto dto)
    {
        var response = new ResponseDto{Status ="error", Message = BadRequest("Check-in failed.").ToString()};
        //TODO
        //1. Create an appropriate DTO
        //2. Implement the correct workflow (Guest -> Reservation -> Checkin)
        if(dto!=null)
        {
             Console.WriteLine($"WALKIN DTO CHECKIN: {dto.CheckIn}");
             Console.WriteLine($"WALKIN DTO CHECKOUT: {dto.CheckOut}");
             
            var reservationDto = new CreateReservationDto
            {
                RoomId = dto.RoomId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email=dto.Email,
                ReservationSource = dto.ReservationSource,
                SpecialRequests=dto.SpecialRequests,
                Phone = dto.Phone,
                IdNumber = dto.IdNumber,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                Guests = dto.Guests,
                PreferenceIds = dto.PreferenceIds,
                Status = ReservationStatuses.CheckedIn,
            };
            //new Reservation
            
            response = await _reservationService.CreateReservation(reservationDto);
            if(response.Status =="success")
            {
                //3. create checkin invoice
                var ans = (ReservationDto) response.Payload; 
                response = await _invoiceService.GenerateCheckInInvoiceAsync(ans.Id);
                //4. Deposit > 0 ? Use invoice ID from response object to make a payment
                //make a payment object here and insert a record  of a deposit for this client 
                ///TODO: Create Repository for Payments, to implement the payments workflow
            }
        }
        return Ok(response);
    }
    
    [HttpPut("checkout")]
    public async Task<ActionResult<ResponseDto>> Checkout(ReservationCheckOutDto dto)
    {
        var response = new ResponseDto{Status ="error", Message = BadRequest("Check-out failed.").ToString()};
        if(dto!= null)
        {
            if(dto.MinibarCharges > 0)
            {
                var  invoicelineItems =new LineItemsDto
                {
                    InvoiceId = 1, // This will be set when the invoice is created
                    Description = "Minibar Charges",
                    Quantity = 1,
                    UnitPrice = dto.MinibarCharges,
                    LineTotal = dto.MinibarCharges
                };
                response = await _invoiceService.AddInvoiceLineItemAsync(dto.Id, invoicelineItems);
            }
            if(dto.DamageCharges > 0)
            {
                var invoicelineItems = new LineItemsDto
                {
                    InvoiceId = 2, // This will be set when the invoice is created
                    Description = "Damage Charges",
                    Quantity = 1,
                    UnitPrice = dto.DamageCharges,
                    LineTotal = dto.DamageCharges 
                };
                response = await _invoiceService.AddInvoiceLineItemAsync(dto.Id, invoicelineItems);
            }
            
            if(response.Status == "success")
            {
                response = await _reservationService.CheckOut(dto.Id);
                //3. create checkout invoice
            }
            if(response.Status !="success" && dto.DamageCharges == 0 && dto.MinibarCharges == 0)
            {
                response = await _reservationService.CheckOut(dto.Id);
            }
            //Finally, settle the invoice
            if(response.Status == "success")
            {
                response = await _invoiceService.SettleInvoiceAsync(dto.Id);
            }
        }
        return Ok(response);
    }
    [HttpGet("checkoutguests")]
    public async Task<ActionResult<ReservationDto?>> GetReservationsPendingCheckOut()
    {
        var response = await _reservationService.GetReservationOnCheckOutAsync();
        return Ok(response);
    }

    [HttpGet("{roomNumber}")]
    public async Task<ActionResult<ReservationDto>> GetCurrentReservationByRoomNumber(string roomNumber)
    {
        var reservation = await _guestService.GetCurrentReservationByGuestIdAsync(roomNumber);
        return Ok(reservation);
    }
}