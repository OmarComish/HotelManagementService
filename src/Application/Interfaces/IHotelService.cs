using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;

public interface IHotelService
{
    Task<IEnumerable<HotelDto>> GetAllHotelsAsync();
    Task<HotelDto?> GetHotelByIdAsync(int id);
    Task<HotelDto> CreateHotelAsync(CreateHotelDto createDto);
    Task<HotelDto?> UpdateHotelAsync(int id, CreateHotelDto updateDto);
    Task<bool> DeleteHotelAsync(int id);
}