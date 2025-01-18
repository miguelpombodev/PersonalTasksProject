using PersonalTasksProject.Context;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Repositories.Implementations;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}