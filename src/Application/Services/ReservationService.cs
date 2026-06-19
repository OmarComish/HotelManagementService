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
        var response = new ResponseDto{Status ="error", Message="Failed to create reservation"};
        try
        {
            //validate room exists and is available
            var room = await _unitOfWork.Rooms.GetByIdAsync(createReservationDto.RoomId);
            
            if (room == null)
            {
                response.Message ="Room not found";
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
                
            var reservation = _mapper.Map<Reservation>(createReservationDto);

            // Calculate total amount
            var nights = (createReservationDto.CheckOut - createReservationDto.CheckIn).Days;
            var roomType = await _unitOfWork.RoomTypes.GetByIdAsync(room.RoomTypeId);
            reservation.TotalAmount = nights * roomType.Price;


            var createdreservation = await _unitOfWork.Reservations.AddAsync(reservation);
              //await _unitOfWork.SaveChangesAsync();

             //Change the room status to reserved
             room.Status = room.Status== RecordStatus.Available? RecordStatus.Reserved: room.Status;
             var roomstatuschange = await _unitOfWork.Rooms.UpdateAsync(room);

             await _unitOfWork.SaveChangesAsync();

              response.Status ="success";
              response.Message = "Reservation created successfully";

            // _logger.LogInformation("Reservation created successfully with ID: {ReservationId}", createdreservation.Id);
            response.Payload = _mapper.Map<ReservationDto>(createdreservation);
            
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


        if (reservationdto.GuestName != null)
            reservation.GuestName = reservationdto.GuestName;

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

        await _unitOfWork.Reservations.UpdateAsync(reservation);
        await _unitOfWork.SaveChangesAsync();

        response.Status = "success";
        response.Message = $"Check-in for guest {reservation.GuestName} successful";
        response.Payload = reservation;

        return response;
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
        response.Message =$"Check-in for guest {reservation.GuestName} successful";
        response.Payload = reservation; //_mapper.Map<ReservationDto>(reservation);

        return response;
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
}