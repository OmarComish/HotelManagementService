using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;
public class TaskService : ITaskService
{
     private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    //private readonly ILogger<TaskService> _logger;

    public TaskService(IUnitOfWork unitOfWork, IMapper mapper/*, ILogger<TaskService> logger*/)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        //_logger = logger;
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
    {
        // Validate hotel exists
        var hotel = await _unitOfWork.Hotels.GetByIdAsync(createTaskDto.HotelId);
        if (hotel == null)
            throw new ArgumentException("Hotel not found");

        // Validate assigned user if provided
        if (createTaskDto.AssignedToId.HasValue)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(createTaskDto.AssignedToId.Value);
            if (employee == null)
                   throw new ArgumentException("Assigned user not found");
             
        }

        //Validate TaskType exists
        var taskTypeExists = await _unitOfWork.TaskTypes.GetByIdAsync(createTaskDto.TaskTypeId);
        if(taskTypeExists == null)
        {
            throw new ArgumentException($"Task type with ID: {createTaskDto.TaskTypeId} does not exist");
        }
        
         
        var task = _mapper.Map<TaskItem>(createTaskDto);
        var createdTask = await _unitOfWork.TaskItems.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

       // _logger.LogInformation("Task created successfully with ID: {TaskId}", createdTask.Id);
        return _mapper.Map<TaskDto>(createdTask);
    }

    public async Task<TaskDto?> GetTaskByIdAsync(int id)
    {
        var task = await _unitOfWork.TaskItems.GetByIdAsync(id);
        return task != null ? _mapper.Map<TaskDto>(task) : null;
    }

    public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
    {
        //var tasks = await _unitOfWork.Tasks.GetAllAsync();
        var tasks = await _unitOfWork.TaskItems.GetAllAsync();
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async Task<IEnumerable<TaskDto>> GetTasksByHotelIdAsync(int hotelId)
    {
        var tasks = await _unitOfWork.TaskItems.FindAsync(t => t.HotelId == hotelId);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async Task<IEnumerable<TaskDto>> GetTasksByStatusAsync(string status)
    {
        if(!Enum.TryParse(status, ignoreCase: true, out RecordStatus parsedStatus))
        {
            throw new ArgumentException("Invalid Status Provided");
        }
        var tasks = await _unitOfWork.TaskItems.FindAsync(t => t.Status == parsedStatus);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async Task<TaskDto> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto)
    {
        var task = await _unitOfWork.TaskItems.GetByIdAsync(id);
        if (task == null)
            throw new ArgumentException("Task not found");

        // Update only provided fields
        if (!string.IsNullOrEmpty(updateTaskDto.Description))
            task.Description = updateTaskDto.Description;

        /*if (!string.IsNullOrEmpty(updateTaskDto.Type))
            task.Type = updateTaskDto.Type;*/

        if (updateTaskDto.AssignedToId.HasValue)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(updateTaskDto.AssignedToId.Value);
            if (user == null)
                throw new ArgumentException("Assigned user not found");
            task.AssignedToId = updateTaskDto.AssignedToId.Value;
        }

        if (updateTaskDto.DueDate.HasValue)
            task.DueDate = updateTaskDto.DueDate.Value;

        if (!string.IsNullOrEmpty(updateTaskDto.Status))
        {
            if(!Enum.TryParse(updateTaskDto.Status, ignoreCase: true, out RecordStatus res))
            {
                throw new ArgumentException("Invalid Status Provided");
            }
             task.Status = res;
        }
           

        if (!string.IsNullOrEmpty(updateTaskDto.Notes))
            task.Notes = updateTaskDto.Notes;

        task.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.TaskItems.UpdateAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _unitOfWork.TaskItems.GetByIdAsync(id);
        if (task == null)
            return false;

        await _unitOfWork.TaskItems.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AssignTaskAsync(int taskId, int userId)
    {
        var task = await _unitOfWork.TaskItems.GetByIdAsync(taskId);
        if (task == null)
            return false;

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("User not found");

        task.AssignedToId = userId;
        if(!Enum.TryParse("Inprogress", ignoreCase: true, out RecordStatus parsedStatus))
        {
            throw new ArgumentException("Invalid Status Provided");
        }
        task.Status = parsedStatus;
        task.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.TaskItems.UpdateAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CompleteTaskAsync(int taskId)
    {
        var task = await _unitOfWork.TaskItems.GetByIdAsync(taskId);
        if (task == null)
            return false;

        if(!Enum.TryParse("Completed", ignoreCase: true, out RecordStatus parsedStatus))
        {
            throw new ArgumentException("Invalid Status provided");
        }

        task.Status = parsedStatus;
        task.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.TaskItems.UpdateAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

