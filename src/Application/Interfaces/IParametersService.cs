using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface IParametersService
{
    Task<IEnumerable<TaskStatusDto>> GetAllRecordStatusAsync();
    Task<IEnumerable<TaskPrioritiesDto>> GetAllPrioritiesAsync();
    Task<IEnumerable<TaskTypeDto>> GetAllTaskTypesAsync();
    Task<ResponseDto> CreateTaskType(CreateTaskTypeDto createTaskTypeDto);
    Task<IEnumerable<RoomTypeDto>> GetRoomTypesAsync();

}