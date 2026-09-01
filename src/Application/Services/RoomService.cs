using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;
public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    //private readonly ILogger<RoomService> _logger;

    public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        //_logger = logger;
    }

    public async Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto)
    {
        // Validate hotel exists
        var hotel = await _unitOfWork.Hotels.GetByIdAsync(createRoomDto.HotelId);
        if (hotel == null)
            throw new ArgumentException("Hotel not found");

        // Check if room number already exists in the hotel
        var existingRoom = await _unitOfWork.Rooms.FirstOrDefaultAsync(r =>
            r.HotelId == createRoomDto.HotelId && r.RoomNumber == createRoomDto.RoomNumber);

        if (existingRoom != null)
            throw new InvalidOperationException("Room number already exists in this hotel");

        var room = _mapper.Map<Room>(createRoomDto);
        
          // Set default status
        room.Status = RecordStatus.Available; 
      
        var createdRoom = await _unitOfWork.Rooms.AddAsync(room);
        await _unitOfWork.SaveChangesAsync();

        //_logger.LogInformation("Room created successfully with ID: {RoomId}", createdRoom.Id);

        return _mapper.Map<RoomDto>(createdRoom);
    }

    public async Task<RoomDto?> GetRoomByIdAsync(int id)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(id);
        return room != null ? _mapper.Map<RoomDto>(room) : null;
    }
    public async Task<RoomDto> GetRoomByRoomNumber(string roomNum)
    {
        var room = await _unitOfWork.Rooms.FindAsync(r => r.RoomNumber == roomNum);
        return  _mapper.Map<RoomDto>(room);
    }

    public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
    {
        var rooms = await _unitOfWork.Rooms.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<RoomDto>>(rooms);
    }

    public async Task<IEnumerable<RoomDto>> GetRoomsByHotelIdAsync(int hotelId)
    {
        var rooms = await _unitOfWork.Rooms.FindAsync(r => r.HotelId == hotelId);
      
        return _mapper.Map<IEnumerable<RoomDto>>(rooms);
    }

    public async Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut)
    {
        var allRooms = await _unitOfWork.Rooms.FindAsync(r => r.HotelId == hotelId && r.Status == RecordStatus.Available);
        var availableRooms = new List<Room>();

        foreach (var room in allRooms)
        {
            var hasConflict = await HasBookingConflictAsync(room.Id, checkIn, checkOut);
            if (!hasConflict)
            {
                availableRooms.Add(room);
            }
        }

        return _mapper.Map<IEnumerable<RoomDto>>(availableRooms);
    }
    public async Task<RoomDto> UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(id);
        if (room == null)
            throw new ArgumentException("Room not found");

        // Update only provided fields
        if (updateRoomDto.Type != 0)
            room.RoomTypeId = updateRoomDto.Type;

        /*if (updateRoomDto.Price.HasValue)
            room.Price = updateRoomDto.Price.Value; */

        if (!string.IsNullOrEmpty(updateRoomDto.Status))
            room.Status = Enum.TryParse<RecordStatus>(updateRoomDto.Status, true, out var status)? room.Status = status: room.Status;
            //room.Status = updateRoomDto.Status;

        room.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Rooms.UpdateAsync(room);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoomDto>(room);
    }
    public async Task<bool> DeleteRoomAsync(int id)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(id);
        if (room == null)
            return false;

        // Check if room has active bookings
        var activeBookings = await _unitOfWork.Reservations.FindAsync(b =>
            b.RoomId == id && (b.Status == ReservationStatuses.Reserved || b.Status == ReservationStatuses.CheckedIn));

        if (activeBookings.Any())
            throw new InvalidOperationException("Cannot delete room with active bookings");

        await _unitOfWork.Rooms.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    private async Task<bool> HasBookingConflictAsync(int roomId, DateTime checkIn, DateTime checkOut)
    {
        var conflictingBookings = await _unitOfWork.Reservations.FindAsync(b =>
            b.RoomId == roomId &&
            b.Status != ReservationStatuses.Cancelled &&
            b.Status != ReservationStatuses.Completed &&
            ((checkIn >= b.CheckIn && checkIn < b.CheckOut) ||
             (checkOut > b.CheckIn && checkOut <= b.CheckOut) ||
             (checkIn <= b.CheckIn && checkOut >= b.CheckOut)));

        return conflictingBookings.Any();
    }
}