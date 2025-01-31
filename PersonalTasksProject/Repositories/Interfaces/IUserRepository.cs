using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<int> UpdateUserAvatarAsync(Guid id, string avatarFilePath);
}