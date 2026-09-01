using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelmanagementService.Application.Services;
public class AmenitiesService : IAmenitiesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AmenitiesService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AmenitiesDto>> GetAllAmenitiesAsync()
    {
        var amenities = await _unitOfWork.Amenities.GetAllAsync();
        return _mapper.Map<IEnumerable<AmenitiesDto>>(amenities);
    }

    public async Task<AmenitiesDto> GetAmenityByIdAsync(int id)
    {
        var amenity = await _unitOfWork.Amenities.GetByIdAsync(id);
        if (amenity == null)
        {
            throw new KeyNotFoundException($"Amenity with ID {id} not found.");
        }
        return _mapper.Map<AmenitiesDto>(amenity);
    }

    public async Task<ResponseDto> CreateAmenity(CreateAmenitiesDto createAmenitiesDto)
    {
        var amenity = _mapper.Map<Amenities>(createAmenitiesDto);
        amenity.CreatedAt = DateTime.UtcNow;
        amenity.CreatedBy = "System"; // You can replace this with the actual user if you have authentication   
        
        await _unitOfWork.Amenities.AddAsync(amenity);
        await _unitOfWork.SaveChangesAsync();
        return new ResponseDto { Status = "success", Message = "Amenity created successfully." };
    }

    public async Task<AmenitiesDto> UpdateAmenitiesAsync(int id, UpdateAmenitiesDto updateAmenitiesDto)
    {
        var amenity = await _unitOfWork.Amenities.GetByIdAsync(id);
        if (amenity == null)
        {
            throw new KeyNotFoundException($"Amenity with ID {id} not found.");
        }
        _mapper.Map(updateAmenitiesDto, amenity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AmenitiesDto>(amenity);
    }

    public async Task<bool> DeleteAmenityAsync(int id)
    {
        var amenity = await _unitOfWork.Amenities.GetByIdAsync(id);
        if (amenity == null)
        {
            throw new KeyNotFoundException($"Amenity with ID {id} not found.");
        }
        _unitOfWork.Amenities.DeleteAsync(amenity.Id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}