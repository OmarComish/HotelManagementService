using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelFlowAPI.Application.Services;
public class GuestPreferenceService : IGuestPreferenceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GuestPreferenceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GuestPreferenceDto> CreateGuestPreferenceAsync(CreateGuestPreferenceDto createGuestPreferenceDto)
    {
        var guestPreference = _mapper.Map<GuestPreferences>(createGuestPreferenceDto);
        var createdGuestPreference = await _unitOfWork.GuestPreferences.AddAsync(guestPreference);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<GuestPreferenceDto>(createdGuestPreference);
    }

    public async Task<GuestPreferenceDto?> GetGuestPreferenceByIdAsync(int id)
    {
        var guestPreference = await _unitOfWork.GuestPreferences.GetByIdAsync(id);
        return guestPreference == null ? null : _mapper.Map<GuestPreferenceDto>(guestPreference);
    }

    public async Task<IEnumerable<GuestPreferenceDto>> GetAllGuestPreferencesAsync()
    {
        var guestPreferences = await _unitOfWork.GuestPreferences.GetAllAsync();
        return _mapper.Map<IEnumerable<GuestPreferenceDto>>(guestPreferences);
    }

    public async Task<GuestPreferenceDto> UpdateGuestPreferenceAsync(int id, UpdateGuestPreferenceDto updateGuestPreferenceDto)
    {
        var existingGuestPreference = await _unitOfWork.GuestPreferences.GetByIdAsync(id);
        if (existingGuestPreference == null)
        {
            throw new KeyNotFoundException($"Guest preference with ID {id} not found.");
        }

        // Map the updated fields from the DTO to the existing entity
        _mapper.Map(updateGuestPreferenceDto, existingGuestPreference);

        await _unitOfWork.GuestPreferences.UpdateAsync(existingGuestPreference);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<GuestPreferenceDto>(existingGuestPreference);
    }

    public async Task<bool> DeleteGuestPreferenceAsync(int id)
    {
        var existingGuestPreference = await _unitOfWork.GuestPreferences.GetByIdAsync(id);
        if (existingGuestPreference == null)
        {
            return false; // Or throw an exception if preferred
        }

        await _unitOfWork.GuestPreferences.DeleteAsync(existingGuestPreference.Id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}