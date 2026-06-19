using HotelManagementService.Core.Entities;

namespace HotelManagementService.Core.Interfaces;
public interface ITaskTypeRepository: IRepository<TaskType>
{
     Task<IEnumerable<TaskType>> GetAllWithDetailsAsync();
     Task<TaskType> GetByIdWithDetailsAsync(int id);
}