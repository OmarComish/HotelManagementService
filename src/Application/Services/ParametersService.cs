
using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;

namespace HotelManagementService.Application.Services;
public class ParameterService: IParametersService
{
    private readonly IMapper _mapper;
    //private readonly ILogger<IParametersService> _logger;
     private readonly IUnitOfWork _unitOfWork;
    public ParameterService(IMapper mapper,IUnitOfWork unitOfWork /*, ILogger<IParametersService> logger*/)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        //_logger = logger;

    }
    public async Task<IEnumerable<TaskStatusDto>> GetAllRecordStatusAsync()
    {
        var recordstatuses = new List<TaskStatusDto>()
        {
           new() {Id = (int)RecordStatus.Inprogress, Name = "InProgress", Description ="Task in progress"},
           new() {Id = (int)RecordStatus.Pending, Name = "pending", Description ="Task on Pending"},
           new() {Id = (int)RecordStatus.Completed, Name = "completed", Description ="Task Completed"},
           new() {Id = (int)RecordStatus.Overdue, Name = "overdue", Description ="Task Overdue"},
        };
        return recordstatuses;
    }
    public async Task<IEnumerable<TaskPrioritiesDto>> GetAllPrioritiesAsync()
    {
        var taskpriorities = new List<TaskPrioritiesDto>()
        {
            new() {Id = (int) Priorities.Low, Name = "low", Description="Low priority task"},
            new() {Id = (int) Priorities.Medium, Name = "medium", Description="Medium priority task"},
            new() {Id = (int) Priorities.High, Name = "high", Description="High priority task"},
            new() {Id = (int) Priorities.Urgent, Name = "urgent", Description="Urgent task"}
        };
        return taskpriorities;
    }
    public async Task<IEnumerable<TaskTypeDto>> GetAllTaskTypesAsync()
    {
        var tasktypes = await _unitOfWork.TaskTypes.GetAllAsync();
        return _mapper.Map<IEnumerable<TaskTypeDto>>(tasktypes);
    }
    public async Task<ResponseDto> CreateTaskType(CreateTaskTypeDto createTaskTypeDto)
    {
        var response = new ResponseDto{Status= "error", Message = "An error occurred while creating task type"};
        if(string.IsNullOrEmpty(createTaskTypeDto.Name) && string.IsNullOrEmpty(createTaskTypeDto.Title))
        {
            throw new ArgumentNullException("Cannot create Task type with null values");
        }
    
        var tasktype = _mapper.Map<TaskType>(createTaskTypeDto);
        tasktype.CreatedBy = "Admin";
        tasktype.CreatedAt = DateTime.UtcNow;

        var createdTaskType = await _unitOfWork.TaskTypes.AddAsync(tasktype);
        await _unitOfWork.SaveChangesAsync();

        // _logger.LogInformation("Task created successfully with ID: {TaskId}", createdTaskType.Id);

        response.Status ="success";
        response.Message = $"Task created successfully with ID:, {createdTaskType.Id}";

        response.Payload = _mapper.Map<TaskTypeDto>(createdTaskType);
        return response;
    }

    public async Task<IEnumerable<RoomTypeDto>> GetRoomTypesAsync()
    {
        var roomtypes = await _unitOfWork.RoomTypes.GetAllAsync();
        return _mapper.Map<IEnumerable<RoomTypeDto>>(roomtypes);
    }
}