using System.Data.Common;
using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;


namespace HotelManagementService.Application.Services;
public class InvoiceService: IInvoiceService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public InvoiceService(IMapper mapper, IUnitOfWork uow)
    {
        _unitOfWork = uow;
        _mapper = mapper;
    }

    public async Task<ResponseDto> GenerateCheckInInvoiceAsync(int reservationId)
    {
        var response = new ResponseDto{Status ="error", Message="Failed to create invoice"};
        try
        {
            var reservation = await _unitOfWork.Reservations.GetWithRoomDetailsByIdAsync(reservationId);
             
            var nights = (reservation.CheckOut - reservation.CheckIn).Days;
            var invoice = new Invoice
            {
                ReservationId = reservationId,
                InvoiceNumber = await GenerateInvoiceNumberAsync(),
                IssuedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow,
                Status = InvoiceStatus.draft,
                PaymentMethod = "Cash",
                LineItems = new List<InvoiceLineItem>
                {
                    new()
                    {
                        Description = $"Room {reservation.Room.RoomNumber} - {nights} night(s)",
                        Quantity = nights,
                        UnitPrice = reservation.Room.RoomType.Price 
                    }
                }
            };  
            invoice.TotalAmount = invoice.LineItems.Sum(l =>l.LineTotal);
            invoice.Status = InvoiceStatus.draft;

            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            response.Status ="success";
            response.Message =$"Invoice {invoice.InvoiceNumber} created successfully";
            response.Payload = _mapper.Map<CreateInvoiceDto>(invoice);

        }
        catch(Exception e)
        {
            response.Message = $"Error occurred while creating invoice. Detail: {e.Message}";
        }

        return response;
    }

    private async Task<string> GenerateInvoiceNumberAsync()
    {
        var count = await _unitOfWork.Invoices.CountAsync();
        return $"INV-{DateTime.UtcNow.Year}-{(count + 1):D5}";
    }
    public async Task<IEnumerable<ReadInvoiceDto>> GetInvoicesAsync()
    {
        var response = await _unitOfWork.Invoices.GetInvoicesAsync();
        return _mapper.Map<IEnumerable<ReadInvoiceDto>>(response);
    }
    public async Task<ResponseDto> GenerateCheckOutInvoiceAsync(int reservationId)
    {
        var response = new ResponseDto{Status ="error", Message="Failed to create invoice"};
        try
        {
           var reservation = await _unitOfWork.Reservations.GetWithRoomDetailsByIdAsync(reservationId);
           var depositInvoice = await _unitOfWork.Invoices.GetByReservationAsync(reservationId);
           var extras = await _unitOfWork.Restaurants.GetByReservationAsync(reservationId);

           var depositPaid = depositInvoice?.Payments.Sum(p =>p.Amount)?? 0;
           var nights      = (reservation.CheckOut - reservation.CheckIn).Days;

           var lineItems   = new List<InvoiceLineItem>
           {
              new(){Description = $"Room {reservation.Room.RoomNumber} - {nights} night(s)",
                    Quantity = nights, UnitPrice = reservation.Room.RoomType.Price}  
           }; 

           foreach(var order in extras)
             lineItems.Add(new(){Description =$"Restaurant order - {order.Items.Count} item(s)",
                                  Quantity = 1, UnitPrice = order.TotalAmount});

            var subtotal = lineItems.Sum(l =>l.LineTotal);

            //Deposit already paid becomes a credit line
            if(depositPaid > 0)
               lineItems.Add(new(){ Description ="Deposit paid on check-in", 
               Quantity = 1, UnitPrice = -depositPaid});

            var invoice = new Invoice
            {
                ReservationId = reservationId,
                InvoiceNumber = await GenerateInvoiceNumberAsync(),
                IssuedDate    = DateTime.UtcNow,
                DueDate       = DateTime.UtcNow.AddDays(1),
                Status        = InvoiceStatus.draft,
                LineItems     = lineItems,
                TotalAmount   = lineItems.Sum(l => l.LineTotal),
                PaymentMethod = "Cash"
            }; 

            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            response.Status = "success";
            response.Message = "Invoice added successfully";
            response.Payload = _mapper.Map<CreateInvoiceDto>(invoice);
        }
        catch (DbException e)
        {
            response.Message = $"An error occured while creating invoice. Details: {e.InnerException}";
        }

        return response;
    }
    public async Task<ResponseDto> AddInvoiceLineItemAsync(int reservationId, LineItemsDto dto)
    {
        //1. get the invoice Id
        var response = new ResponseDto{Status ="error", Message="Failed to add invoice line item"};
        try
        {
            Console.WriteLine($"Adding line item for reservation ID: {reservationId}");
            var invoice= await _unitOfWork.Invoices.GetByReservationAsync(reservationId);
            if(invoice != null)
            {
               Console.WriteLine($"Invoice found for ID: {reservationId}  ...Adding line item: {dto.Description}, Quantity: {dto.Quantity}, UnitPrice: {dto.UnitPrice}");
                var lineItem = _mapper.Map<InvoiceLineItem>(dto);
                lineItem.InvoiceId = invoice.Id;

                var results = await _unitOfWork.InvoiceLineItems.AddAsync(lineItem);
                await _unitOfWork.SaveChangesAsync();
                response.Payload = results;
                response.Status = "success";
                response.Message = "line item added successfully";    
                Console.WriteLine($"Adding line item for reservation ID: {reservationId} successfully completed");
            }
           Console.WriteLine($"failed to find Invoice for reservation ID : {reservationId}.. No line item added");
        }
        catch (Exception e)
        {
            response.Message = e.InnerException?.Message;
        }
        return response;
    }
    public async Task<ResponseDto> SettleInvoiceAsync(int reservationId)
    {
        var response = new ResponseDto{Status ="error", Message="Failed to settle invoice"};
        try
        {
            var invoice = await _unitOfWork.Invoices.GetByReservationAsync(reservationId);
            if(invoice != null)
            {
                invoice.Status = InvoiceStatus.paid;
                _unitOfWork.Invoices.Update(invoice);
                await _unitOfWork.SaveChangesAsync();
                response.Status = "success";
                response.Message = $"Invoice {invoice.InvoiceNumber} settled successfully";
            }
            else
            {
                response.Message = $"No invoice found for reservation ID: {reservationId}";
            }
        }
        catch (Exception e)
        {
            response.Message = $"An error occurred while settling the invoice. Details: {e.Message}";
        }
        return response;
    }
}