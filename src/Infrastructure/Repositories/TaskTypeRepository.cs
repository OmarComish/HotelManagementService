using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Repositories;
public class TaskTypeRepository: GenericRepository<TaskType>, ITaskTypeRepository
{
    public TaskTypeRepository(HotelDbContext context):base(context){}
    public async Task<IEnumerable<TaskType>> GetAllWithDetailsAsync()
    {
        return await _context.TaskTypes.ToListAsync();
    }

    public async Task<TaskType> GetByIdWithDetailsAsync(int id)
    {
        return await _context.TaskTypes.FirstOrDefaultAsync(r => r.Id == id);
    }
}