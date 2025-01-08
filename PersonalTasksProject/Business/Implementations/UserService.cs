using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Business.Implementations;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _userRepository.AddAsync(user);
        return user;
    }

    public Task<User> UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }
}