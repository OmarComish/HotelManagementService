using HotelManagementService.Application.DTOs;

public interface IRoomService
{
    Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto);
    Task<RoomDto?> GetRoomByIdAsync(int id);
    Task<RoomDto> GetRoomByRoomNumber(string roomnumber);
    Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
    Task<IEnumerable<RoomDto>> GetRoomsByHotelIdAsync(int hotelId);
    Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut);
    Task<RoomDto> UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto);
    Task<bool> DeleteRoomAsync(int id);
}