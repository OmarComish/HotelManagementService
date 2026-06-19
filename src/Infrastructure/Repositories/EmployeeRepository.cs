using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;

namespace HotelManagementService.Infrastructure.Repositories;
public class EmployeeRepository: GenericRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(HotelDbContext context): base(context)
    {
        
    }
}