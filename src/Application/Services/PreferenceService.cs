using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;
public class PreferenceService : IPreferenceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PreferenceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PreferenceDto> CreatePreferenceAsync(CreatePreferenceDto createPreferenceDto)
    {
        var preference = _mapper.Map<Preferences>(createPreferenceDto);
        preference.CreatedAt = DateTime.UtcNow;
        preference.CreatedBy = "System"; // You can replace this with the actual user creating the preference
        //preference.Status = RecordStatus.Active; // Assuming you have an enum for record status
        await _unitOfWork.Preferences.AddAsync(preference);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PreferenceDto>(preference);
    }

    public async Task<PreferenceDto?> GetPreferenceByIdAsync(int id)
    {
        var preference = await _unitOfWork.Preferences.GetByIdAsync(id);
        return preference == null ? null : _mapper.Map<PreferenceDto>(preference);
    }

    public async Task<IEnumerable<PreferenceDto>> GetAllPreferencesAsync()
    {
        var preferences = await _unitOfWork.Preferences.GetAllAsync();
        return _mapper.Map<IEnumerable<PreferenceDto>>(preferences);
    }

    public async Task<PreferenceDto> UpdatePreferenceAsync(int id, UpdatePreferenceDto updatePreferenceDto)
    {
        var preference = await _unitOfWork.Preferences.GetByIdAsync(id);
        if (preference == null)
        {
            throw new KeyNotFoundException($"Preference with ID {id} not found.");
        }
        preference.Name = updatePreferenceDto.Name;
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PreferenceDto>(preference);
    }

    public async Task<bool> DeletePreferenceAsync(int id)
    {
        var preference = await _unitOfWork.Preferences.GetByIdAsync(id);
        if (preference == null)
        {
            return false;
        }
        await _unitOfWork.Preferences.DeleteAsync(preference.Id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}