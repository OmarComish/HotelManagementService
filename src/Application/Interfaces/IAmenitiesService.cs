using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;

public interface IAmenitiesService
{
    Task<IEnumerable<AmenitiesDto>> GetAllAmenitiesAsync();
    Task<AmenitiesDto> GetAmenityByIdAsync(int id);
    Task<ResponseDto> CreateAmenity(CreateAmenitiesDto createAmenitiesDto);
    Task<AmenitiesDto> UpdateAmenitiesAsync(int id, UpdateAmenitiesDto updateAmenitiesDto);
    Task<bool> DeleteAmenityAsync(int id);
}