using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;
public class RestaurantService : IRestaurantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public RestaurantService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> AddMenuItemAsync(int hotelId, CreateMenuItemDto dto)
    {
        // Implementation for adding menu item
        throw new NotImplementedException();
    }

    public async Task<ResponseDto> UpdateMenuItemAsync(int menuItemId, CreateMenuItemDto dto)
    {
        // Implementation for updating menu item
        throw new NotImplementedException();
    }

    public async Task<ResponseDto> DeleteMenuItemAsync(int menuItemId)
    {
        // Implementation for deleting menu item
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<MenuItemDto>> GetMenuItemsByHotelIdAsync(int hotelId)
    {
        var results = await _unitOfWork.MenuItems.GetAllAsync();
        
        //Console.WriteLine($"Total menu items for hotel {hotelId}: {results.Count()}");
        if (results == null)
        {
            throw new KeyNotFoundException($"No restaurant found for hotel {hotelId}");
        }

        if (results == null)
        {
            return Enumerable.Empty<MenuItemDto>();
        }
        
        return _mapper.Map<IEnumerable<MenuItemDto>>(results);
    }
    public async Task<ResponseDto> AddOrderAsync(int hotelId, CreateRestaurantOrderDto dto)
    {
        
        try
        {
            //Console.WriteLine("STEP 1 - AddOrderAsync started");
            //1. Validate table belongs to hotel
            var table = await _unitOfWork.RestaurantTables.GetByIdWithDetailsAsync(dto.TableId);

            //Console.WriteLine($"STEP 2 - Table result: {table?.Id}");
            if (table == null )
            {
                return new ResponseDto { Status = "error", Message = $"Table with ID {dto.TableId} not found in this hotel."};
            }

            //Console.WriteLine("STEP 3 - Table validation passed");

            //2. Validate Guest belongs to hotel or provide a default guest if not provided

             //Console.WriteLine("STEP 4 - Starting guest validation");
             //Console.WriteLine($"GuestId: {dto.GuestId}");

            if(dto.GuestId.HasValue)
            {
                try
                {
                    //Console.WriteLine($"STEP 5.1 - Guest before lookup : {dto.GuestId}");

                    //var guest = await _unitOfWork.GuestPreferences.GetByIdWithDetailsAsync(dto.GuestId.Value);//(g=>g.Id==dto.GuestId.Value);//GetByIdWithDetailsAsync(dto.GuestId.Value);
                    var guest = await _unitOfWork.Guests.GetByIdAsync(dto.GuestId.Value);

                    //Console.WriteLine($"STEP 5.2 - Guest lookup result: {guest.Id}");
                    if (guest == null )//|| guest.HotelId != hotelId || guest.Status != "Approved")
                    {
                        // Provide a default guest if none is specified
                        dto.GuestId = 1; // Assuming the default guest has ID 1
                        //return new ResponseDto { Status = "error", 
                        //Message = $"Guest with ID {dto.GuestId} is not approved or does not belong to this hotel."};
                    }
                }
                catch(Exception ex)
                {
                     //Console.WriteLine($"GUEST LOOKUP ERROR: {ex.Message}");
                     //Console.WriteLine($"GUEST LOOKUP INNER ERROR: {ex.InnerException?.Message}");
                }

            }
            else
            {
                // Provide a default guest if none is specified
                dto.GuestId = 1; // Assuming the default guest has ID 1
            }
             //Console.WriteLine($"STEP 6 - Guest validation passed. Final GuestId: {dto.GuestId}");
            //3. If charging to room, find the guest's active reservation and invoice
             Invoice?roomInvoice = null;

             //Console.WriteLine("STEP 7 - Checking payment method");
             //Console.WriteLine($"PaymentMethod received: [{dto.PaymentMethod}]");

            if(dto.PaymentMethod.Equals("room-charge", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("STEP 8 - Charge to room condition PASSED");

                var reservation = await _unitOfWork.Reservations.GetActiveReservationByGuestAsync(dto.GuestId!.Value);
                
                 Console.WriteLine($"STEP 9 - Reservation result: {reservation?.Id}");

                if(reservation ==null)
                {
                    return new ResponseDto
                    {
                       Status = "error",
                       Message = "The guest does not have an active checked-in reservation."   
                    };
                }

                roomInvoice = await _unitOfWork.Invoices.GetByReservationAsync(reservation.Id);

               // Console.WriteLine($"STEP 10 - Invoice result: {roomInvoice?.Id}");
                
               
                if(roomInvoice ==null)
                {
                    return new ResponseDto
                    {
                       Status = "error",
                       Message = "No invoice was found for the guest's active reservation."  
                    };
                }
            }

            //3. Validate menuitem exists and belongs to hotel
            var menuItemIds = dto.Items.Select(i => i.MenuItemId).ToList();
            var menuItems = (await _unitOfWork.MenuItems.
               FindAsync(mi => menuItemIds.Contains(mi.Id) && mi.HotelId == hotelId)).ToDictionary(m=>m.Id);

            var missingIds = menuItemIds.Except(menuItems.Keys).ToList();
            if(missingIds.Any())
            {
                return new ResponseDto { Status = "error", 
                Message = $"Menu items with IDs {string.Join(", ", missingIds)} not found in this hotel."};
            }

            var unavailableItems = menuItems.Values.Where(mi => !mi.IsAvailable).Select(mi => mi.Name).ToList();
            
            if(unavailableItems.Any())
            {
                return new ResponseDto { Status = "error", 
                Message = $"Menu items {string.Join(", ", unavailableItems)} are currently not available."};
            }

            //4. Build orderItems and Calculate total amount
            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach(var itemDto in dto.Items)
            {
                var menuItem = menuItems[itemDto.MenuItemId];
                orderItems.Add(new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    Quantity = itemDto.Quantity,
                    Price = menuItem.Price,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                });
                totalAmount += menuItem.Price * itemDto.Quantity;
            }
    

            //5. Create and Save the order
            var order = _mapper.Map<RestaurantOrder>(dto);
            order.TotalAmount = totalAmount;
            order.OrderNumber = GetOrderNumber(dto.OrderTypeId);
            order.Status = "Available";
            order.Items = orderItems;
            order.OrderTypeId = dto.OrderTypeId;
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            order.CreatedBy = "Admin"; // Replace with actual user info if available 

            //6. Add restaurant charge to room invoice  
           // Console.WriteLine($"STEP 11 - Invoice line items computation: {roomInvoice?.Id}");
            if(roomInvoice!=null)
            {
                var lineItem = new InvoiceLineItem
                {
                    InvoiceId = roomInvoice.Id,
                    Description = $"Restaurant Order - {order.OrderNumber}",
                    Quantity = 1,
                    UnitPrice = totalAmount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                Console.WriteLine($"STEP 12 - Adding Invoice line items");
                //roomInvoice.LineItems.Add(lineItem);
                //await _unitOfWork.InvoiceLineItems.AddAsync(lineItem);
                await AddInvoiceLineItem(lineItem);
               
                roomInvoice.TotalAmount += totalAmount;

                
            }

            await _unitOfWork.RestaurantOrders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            //Console.WriteLine($"STEP 13 - Adding Invoice line item and Order items...");

            //6. Return mapped to DTO
            var orderDto = _mapper.Map<RestaurantOrderDto>(order);
            return new ResponseDto { Status = "success", Message = "Restaurant order created successfully.", Payload = orderDto };
        }
        catch (Exception ex)
        {
            return new ResponseDto 
            { 
                Status = "error", 
                Message = "An unexpected error occurred while creating the restaurant order: " + ex.Message
            };
        }
    }
    public async Task<IEnumerable<RestaurantOrderDto>> GetAllOrdersAsync()
    {
        var orders = await _unitOfWork.RestaurantOrders.GetRestaurantOrdersAsync();

        var orderTypeValue = orders.FirstOrDefault()?.OrderType; // What is the type and value?
        //Console.WriteLine($"OrderType: {orderTypeValue} - Type: {orderTypeValue?.GetType()}");    
        
        return _mapper.Map<IEnumerable<RestaurantOrderDto>>(orders);
    }
    public async Task<RestaurantOrderDto> GetOrderByIdAsync(int orderId)
    {
        var order = await _unitOfWork.RestaurantOrders.GetRestaurantOrderByIdAsync(orderId);
        if (order == null)
        {
            return null;
        }
        return _mapper.Map<RestaurantOrderDto>(order);
    }
    private string GetOrderNumber(int orderTypeId)
    {
        int count = _unitOfWork.RestaurantOrders.CountRestaurantOrdersAsync(orderTypeId).Result;
        string abbreviation = _unitOfWork.OrderTypes.GetOrderTypeByIdAsync(orderTypeId).Result.Abbreviation?.ToUpper() ?? "GEN";
        string orderId  = $"ORD-{abbreviation}-01";
        if(count > 0)  orderId= $"ORD-{abbreviation}-0{count + 1}";
      
        return orderId;
    }
    private async Task<ResponseDto> AddInvoiceLineItem(InvoiceLineItem dto)
    {
        Console.WriteLine("Reached AddInvoiceLineItem Private method...");
        var response = new ResponseDto{Status ="error", Message="An error occurred while adding invoice line items"};
        try
        {
            if(dto!=null)
            {
                Console.WriteLine("AddInvoiceLineItem action started...");
                dto.CreatedAt = DateTime.UtcNow;
                dto.CreatedBy ="admin";
                await _unitOfWork.InvoiceLineItems.AddAsync(dto);
                await _unitOfWork.SaveChangesAsync();

                response.Status ="success";
                response.Message = "Invoice line items added successfully!";

               // Console.WriteLine("***AddInvoiceLineItem action completed***");
            }
        }
        catch(Exception ex)
        {
            response.Message = $"Ann error occurred while adding invoice line item {ex.InnerException?.Message}";
              //Console.WriteLine($"GUEST LOOKUP ERROR: {ex.Message}");
            //Console.WriteLine($"GUEST LOOKUP INNER ERROR: {ex.InnerException?.Message}");
        }

        return response;
    }

}