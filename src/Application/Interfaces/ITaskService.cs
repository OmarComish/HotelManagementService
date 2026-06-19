using HotelManagementService.Application.DTOs;

namespace HotelManagementService.Application.Interfaces;
public interface ITaskService
{
    Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto);
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<IEnumerable<TaskDto>> GetAllTasksAsync();
    Task<IEnumerable<TaskDto>> GetTasksByHotelIdAsync(int hotelId);
    Task<IEnumerable<TaskDto>> GetTasksByStatusAsync(string status);
    Task<TaskDto> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
    Task<bool> DeleteTaskAsync(int id);
    Task<bool> AssignTaskAsync(int taskId, int userId);
    Task<bool> CompleteTaskAsync(int taskId);
}