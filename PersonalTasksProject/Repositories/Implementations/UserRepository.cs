using Microsoft.EntityFrameworkCore;
using PersonalTasksProject.Context;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Repositories.Implementations;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly DbSet<User> _userDbSet;

    public UserRepository(AppDbContext context) : base(context)
    {
        _userDbSet = context.Set<User>();
    }

    public async Task<int> UpdateUserAvatarAsync(Guid id, string avatarFilePath)
    {
        var affectedRow = await _userDbSet.Where(user => user.Id == id).ExecuteUpdateAsync(setters => setters.SetProperty(user => user.AvatarUrl, avatarFilePath));
        
        return affectedRow;
    }
}