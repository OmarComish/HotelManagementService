using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;

public class HotelService : IHotelService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HotelService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HotelDto>> GetAllHotelsAsync()
    {
        var hotels = await _unitOfWork.Hotels.GetAllAsync();
        return _mapper.Map<IEnumerable<HotelDto>>(hotels);
    }

    public async Task<HotelDto?> GetHotelByIdAsync(int id)
    {
        var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);
        return hotel == null ? null : _mapper.Map<HotelDto>(hotel);
    }

    public async Task<HotelDto> CreateHotelAsync(CreateHotelDto createDto)
    {
        var exists = await _unitOfWork.Hotels.ExistsAsync(createDto.Name, createDto.City, createDto.Country);
        if (exists)
            throw new InvalidOperationException("Hotel with same name, city and country already exists.");

        var hotel = _mapper.Map<Hotel>(createDto);
        await _unitOfWork.Hotels.AddAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<HotelDto>(hotel);
    }

    public async Task<HotelDto?> UpdateHotelAsync(int id, CreateHotelDto updateDto)
    {
        var existingHotel = await _unitOfWork.Hotels.GetByIdAsync(id);
        if (existingHotel == null) return null;

        _mapper.Map(updateDto, existingHotel);
        existingHotel.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Hotels.Update(existingHotel);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<HotelDto>(existingHotel);
    }

    public async Task<bool> DeleteHotelAsync(int id)
    {
        var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);
        if (hotel == null) return false;

        _unitOfWork.Hotels.Delete(hotel);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}