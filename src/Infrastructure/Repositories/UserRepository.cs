using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;

namespace HotelManagementService.Infrastructure.Repositories;
public class UserRepository: GenericRepository<User>, IUsersRepository
{
    public UserRepository(HotelDbContext context): base(context){}
}