using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Business.Interfaces;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<User> DeleteUserAsync(int id);
}