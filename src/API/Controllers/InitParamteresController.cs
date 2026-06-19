using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementService.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class InitParametersController: ControllerBase
{
    private readonly ILogger<InitParametersController> _logger;
    private readonly IParametersService _parameterservice;
    public InitParametersController(IParametersService parameterservice, ILogger<InitParametersController> logger)
    {
        _logger = logger;
        _parameterservice = parameterservice;
    }
    [HttpGet]
    public async Task<ActionResult<TaskStatusDto>> GetAllTaskStatuses()
    {
        var response = await _parameterservice.GetAllRecordStatusAsync();
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<TaskPrioritiesDto>> GetAllTaskPriorities()
    {
        var response = await _parameterservice.GetAllPrioritiesAsync();
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<TaskTypeDto>> GetAllTaskTypes()
    {
        var response = await _parameterservice.GetAllTaskTypesAsync();
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<RoomTypeDto>> GetRoomTypes()
    {
        var response = await _parameterservice.GetRoomTypesAsync();
        return Ok(response);
    }
    [HttpPost]
    public async Task<ActionResult<ResponseDto>> CreateTaskType(CreateTaskTypeDto createtasktypeDto)
    {
        var response = new ResponseDto {Status ="error", Message = BadRequest("We could not create Task type").ToString()};
        if(createtasktypeDto!=null)
        {
            response = await _parameterservice.CreateTaskType(createtasktypeDto);
        }
        return Ok(response);
    }
}