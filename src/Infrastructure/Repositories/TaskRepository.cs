using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;

namespace HotelManagementService.Infrastructure.Repositories;
public class TaskRepository: GenericRepository<TaskItem>, ITaskRepository
{
    public TaskRepository(HotelDbContext context):base(context){}
    
}