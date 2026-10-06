using System.Reflection.Metadata;
using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;


namespace HotelManagementService.Application.Services;
public class ReservationService : IReservationService
{
     private readonly IMapper _mapper;
     //private readonly ILogger<ReservationService> _logger;
     private readonly IUnitOfWork _unitOfWork;

    public ReservationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> CreateReservation(CreateReservationDto createReservationDto)
    {
        Console.WriteLine($"RESERVATION STATUS {createReservationDto.Status}");
        //this workflow is missing Adding of a guest first before creating a reservation
        //once adding guest is successful, add the reservation
        var response = new ResponseDto{Status ="error", Message="Failed to create reservation"};
        try
        {

            //Add guest details first
            var guestDto = new CreateGuestDto
            {
                FirstName = createReservationDto.FirstName,
                LastName = createReservationDto.LastName,
                IDNumber = createReservationDto.IdNumber,
                PhoneNumber = createReservationDto.Phone,
                Email = createReservationDto.Email,
                PreferenceIds = createReservationDto.PreferenceIds,
            };
            
            int guestId = await AddGuest(guestDto);

            if(guestId != 0)
            {
                //validate room exists and is available
                var room = await _unitOfWork.Rooms.GetByIdAsync(createReservationDto.RoomId);
                
                if (room == null)
                {
                    response.Status = "error";
                    response.Message ="Reservation failed. Room not found";
                    return response;
                }
                    
                //check for conflicts
                var hasConflict = await HasReservationConflictAsync(
                    createReservationDto.RoomId,
                    createReservationDto.CheckIn,
                    createReservationDto.CheckOut);

                if (hasConflict)
                {
                    response.Message ="Room is not available for the selected dates";
                    return response;
                }
                
              // Console.WriteLine($"BEFORE MAPPING: {createReservationDto.Status}");

                var reservation = _mapper.Map<Reservation>(createReservationDto);
             
                reservation.Status = createReservationDto.Status;

               //Assign the Foreign Key Guest ID to link Guest with their Reservation
             
                //reservation.GuestId = guestId;

                //Assign status
                //reservation.Status = reservation.Status==ReservationStatuses.CheckedIn? reservation.Status:ReservationStatuses.Reserved;
                //reservation.Status = status;

                // Calculate total amount

                reservation.GuestId = guestId;
                var nights = (createReservationDto.CheckOut - createReservationDto.CheckIn).Days;
                var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(room.RoomTypeId);
                reservation.TotalAmount = nights * roomType.Price;


                var createdreservation = await _unitOfWork.Reservations.AddAsync(reservation);
                //await _unitOfWork.SaveChangesAsync();

                //Change the room status to reserved
                room.Status = createReservationDto.Status== ReservationStatuses.CheckedIn? RecordStatus.Occupied: room.Status;
                var roomstatuschange = await _unitOfWork.Rooms.UpdateAsync(room);

                await _unitOfWork.SaveChangesAsync();

                response.Status ="success";
                response.Message = "Reservation created successfully";

                // _logger.LogInformation("Reservation created successfully with ID: {ReservationId}", createdreservation.Id);
                response.Payload = _mapper.Map<ReservationDto>(createdreservation);
            } 
            else
            {
                response.Message = "Failed to create reservation";
            }

            
        }
        catch (Exception ex)
        {
          response.Message = ex.Message;
        }

        return response;
    }
    public async Task<List<ReservationDto>> GetAllReservations()
    {
        var reservations = await _unitOfWork.Reservations.GetWithRoomDetailsAsync();   
        return _mapper.Map<List<ReservationDto>>(reservations);
    }
    public async Task<ReservationDto> UpdateReservationAsync(UpdateReservationDto reservationdto)
    {
        var response = new ResponseDto { Status = "error", Message = "Failed to save changes to reservation" };
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(reservationdto.Id) 
        ?? throw new Exception("Resevation not found!");


        //use current date for checkin, the assumption is that this process is initiated when the customer has 
        //physically arrived on the premises and has confirmed checkin
        if(reservationdto.CheckIn.HasValue)
           reservation.CheckIn = reservationdto.CheckIn.Value; // == default? DateTime.UtcNow: reservation.CheckIn;
        else if (reservation.CheckIn == default)
           reservation.CheckIn = DateTime.UtcNow; //optional: set default when checking in

        if(reservationdto.CheckOut.HasValue && reservationdto.CheckOut.Value > DateTime.UtcNow)
        {
            reservation.CheckOut = reservationdto.CheckOut.Value;
        }
        
        if(reservation.SpecialRequests != null)
            reservation.SpecialRequests = reservationdto.SpecialRequests; // ?? reservation.SpecialRequests;

        if(reservation.ReservationSource !=null)   
            reservation.ReservationSource = reservationdto.ReservationSource; // ?? reservation.ReservationSource;
        
        if(reservation.Phone != null)
           reservation.Phone = reservationdto.Phone; // ?? reservation.Phone;

        if(reservation.Email != null)
           reservation.Email = reservationdto.Email; // ?? reservation.Email;

        if (!string.IsNullOrEmpty(reservationdto.Status)
           && Enum.TryParse<ReservationStatuses>( reservationdto.Status, true, out var status))
        {
            reservation.Status = status;
        }


       /* if (reservationdto.GuestId != null)
            reservation.GuestId = reservationdto.GuestName;*/

        if (reservationdto.RoomId.HasValue && reservationdto.RoomId.Value != 0)
            reservation.RoomId = reservationdto.RoomId.Value;

        reservation.UpdatedAt = DateTime.UtcNow;

        var room = await _unitOfWork.Rooms.GetByIdAsync(reservation.RoomId);
     
         reservation.TotalAmount = await CalculateRoomCost(reservation.CheckIn, 
            reservation.CheckOut, room.RoomTypeId);
 
        await _unitOfWork.Reservations.UpdateAsync(reservation);
        

        //update the room status accordingly
        /*if (Enum.TryParse<RecordStatus>(reservation.Status, true, out var status) 
            && status == RecordStatus.Cancelled)*/
        if(reservation.Status == ReservationStatuses.Cancelled)
        {
            room.Status = RecordStatus.Available;
            await _unitOfWork.Rooms.UpdateAsync(room);
        }
        
        
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ReservationDto>(reservation);
    }
    public async Task<ResponseDto> CheckIn(CheckInDto dto)
    {
        var response = new ResponseDto { Status = "error", Message = "Failed to create reservation" };
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(dto.ReservationId) 
            ?? throw new Exception("Reservation not found!");

        // Update CheckIn
        if (dto.CheckIn.HasValue)
            reservation.CheckIn = dto.CheckIn.Value;
        else if (reservation.CheckIn == default)
            reservation.CheckIn = DateTime.UtcNow;

        // Update CheckOut
        if (dto.CheckOut.HasValue && dto.CheckOut.Value > DateTime.UtcNow)
            reservation.CheckOut = dto.CheckOut.Value;

        // Update string properties – check dto, not reservation
        if (dto.SpecialRequests != null)
            reservation.SpecialRequests = dto.SpecialRequests;
        
        reservation.Status = ReservationStatuses.CheckedIn;

        reservation.UpdatedAt = DateTime.UtcNow;

        // Recalculate total amount
        var room = await _unitOfWork.Rooms.GetByIdAsync(reservation.RoomId);
        reservation.TotalAmount = await CalculateRoomCost(reservation.CheckIn, reservation.CheckOut, room.RoomTypeId);
        
        bool roomstatuschanged = await ChangeRoomStatus(RecordStatus.Occupied, reservation.RoomId);

        if(roomstatuschanged)
        {
            await _unitOfWork.Reservations.UpdateAsync(reservation);
            await _unitOfWork.SaveChangesAsync();

            response.Status = "success";
            response.Message = $"Check-in for guest {reservation.GuestId} successful";
            response.Payload = reservation;
        }
        
        return response;
    }
    public async Task<ResponseDto> CheckOut(int reservationId)
    {
        var response = new ResponseDto { Status = "error", Message = "Failed to check out" };
        //Check if reservation exists
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId) 
            ?? throw new Exception("Reservation not found!");

        // Update status to CheckedOut
        reservation.Status = ReservationStatuses.CheckedOut;
        reservation.UpdatedAt = DateTime.UtcNow;

        // Change room status to Available
        bool roomstatuschanged = await ChangeRoomStatus(RecordStatus.Available, reservation.RoomId);

        if(roomstatuschanged)
        {
            await _unitOfWork.Reservations.UpdateAsync(reservation);
            await _unitOfWork.SaveChangesAsync();

            response.Status = "success";
            response.Message = $"Check-out for guest {reservation.GuestId} successful";
            response.Payload = reservation;
        }
        
        return response;
    }
    private async Task<bool> ChangeRoomStatus(RecordStatus status, int roomId)
    {
        bool success = false;
        var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
        if(room != null)
        {
            room.Status = status;
            await _unitOfWork.Rooms.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync();
            success = true;
        }
        return success;
    }
    public async Task<ResponseDto> CheckInII(CheckInDto dto)
    {
        var response = new ResponseDto{Status ="error", Message="Failed to create reservation"};
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(dto.ReservationId) ?? throw new Exception("Resevation not found!");

       if(dto.CheckIn.HasValue)
           reservation.CheckIn = dto.CheckIn.Value; // == default? DateTime.UtcNow: reservation.CheckIn;
        else if (reservation.CheckIn == default)
           reservation.CheckIn = DateTime.UtcNow; //optional: set default when checking in

        if(dto.CheckOut.HasValue && dto.CheckOut.Value > DateTime.UtcNow)
        {
            reservation.CheckOut = dto.CheckOut.Value;
        }
        
        if(reservation.SpecialRequests != null)
            reservation.SpecialRequests = dto.SpecialRequests; // ?? reservation.SpecialRequests;
        

        var room = await _unitOfWork.Rooms.GetByIdAsync(reservation.RoomId);

        //reservation.SpecialRequests = dto.SpecialRequests ?? reservation.SpecialRequests;
        //reservation.ReservationSource = dto.ReservationSource ?? reservation.ReservationSource;
        
        //reservation.SpecialRequests =  reservation.Email;
        reservation.UpdatedAt = DateTime.UtcNow;

        reservation.TotalAmount = await CalculateRoomCost(reservation.CheckIn, 
            reservation.CheckOut, room.RoomTypeId);

        //reservation.Status = reservation.Status;
        //reservation.ReservationSource =  reservation.ReservationSource;
        //reservation.GuestName = reservation.GuestName;
        //reservation.RoomId = reservation.RoomId;
        
        await _unitOfWork.Reservations.UpdateAsync(reservation);
        await _unitOfWork.SaveChangesAsync();
        

        response.Status ="success";
        response.Message =$"Check-in for guest {reservation.GuestId} successful";
        response.Payload = reservation; //_mapper.Map<ReservationDto>(reservation);

        return response;
    }
    public async Task<IEnumerable<ReservationDto>?> GetReservationOnCheckOutAsync()
    {
        var reservation = await _unitOfWork.Reservations.GetAllReservationsPendingCheckOutAsync();
        return reservation != null ? _mapper.Map<IEnumerable<ReservationDto>>(reservation) : null;
    }
    private async Task<decimal> CalculateRoomCost(DateTime checkIn, DateTime checkOut, int roomTypeId)
    {
            // Calculate total amount
            var nights = (checkOut - checkIn).Days;
            var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(roomTypeId);
            decimal totalAmount = nights * roomType.Price;
            return totalAmount;
    }
    private async Task<bool> HasReservationConflictAsync(int roomId, DateTime checkIn, DateTime checkOut)
    {
        var conflictingReservations = await _unitOfWork.Reservations.FindAsync(b =>
            b.RoomId == roomId &&
            b.Status != ReservationStatuses.Cancelled &&
            b.Status != ReservationStatuses.Completed &&
            ((checkIn >= b.CheckIn && checkIn < b.CheckOut) ||
             (checkOut > b.CheckIn && checkOut <= b.CheckOut) ||
             (checkIn <= b.CheckIn && checkOut >= b.CheckOut)));

        return conflictingReservations.Any();
    }
    private async Task<int> AddGuest(CreateGuestDto createGuestDto)
    {

        //var response = new ResponseDto { Status = "error", Message = "Failed to add guest information" };
        int guestId = 0;
        try
        {
            //Step 1: Check if Guest already exist
            var existingGuest = await _unitOfWork.Guests.FirstOrDefaultAsync(g=>
            (!string.IsNullOrEmpty(createGuestDto.IDNumber) && g.IDNumber == createGuestDto.IDNumber)
            || (!string.IsNullOrEmpty(createGuestDto.PhoneNumber) && g.PhoneNumber == createGuestDto.PhoneNumber)
            || (!string.IsNullOrEmpty(createGuestDto.Email) && g.Email == createGuestDto.Email));
           
            if(existingGuest!=null)
            {
                return guestId = existingGuest.Id;
            }

            //Step 2: Map basic Guest fields
            var guest = _mapper.Map<Guest>(createGuestDto);

            Console.WriteLine($"Guest debug info========================== {guest.Id}");

            //Step 2: Save to database
            var createdGuest = await _unitOfWork.Guests.AddAsync(guest);
            await _unitOfWork.SaveChangesAsync();
            
            //response.Status ="success";
            //response.Message = $"Adding guest data for {createGuestDto.FirstName} successful";
            //response.Payload = createdGuest.Id;

             guestId = createdGuest.Id;

            //Step 3: Add Guest Preferences if any
            Console.WriteLine("preference IDS data");
            Console.WriteLine(createGuestDto.PreferenceIds);

            if(createGuestDto.PreferenceIds != null && createGuestDto.PreferenceIds.Any())
            {
                Console.WriteLine("Adding Preferences...");
                Console.Write(createGuestDto.PreferenceIds );
                var preferences = createGuestDto.PreferenceIds
                .Select(id =>new GuestPreferences
                {
                    GuestId = createdGuest.Id,
                    PreferenceId = id
                }).ToList();

                await _unitOfWork.GuestPreferences.AddRangeAsync(preferences);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch( Exception e)
        {
            Console.WriteLine(e.InnerException?.Message);
        }
 
        return guestId;
    }
}