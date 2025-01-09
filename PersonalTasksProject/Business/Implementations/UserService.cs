using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Extensions;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Business.Implementations;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        User result = await _userRepository.GetByIdAsync(id);

        if (result is null)
        {
            return null;
        }

        return result;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _userRepository.AddAsync(user);
        return user;
    }

    public Task<User> UpdateUserAsync(User user, CreatedUserResponseDto body)
    {
        user.UpdateFromDto(body);
        _userRepository.UpdateAsync(user);
        return Task.FromResult(user);
    }
    

    public async Task<bool?> DeleteUserAsync(Guid id)
    {
        User user = await _userRepository.GetByIdAsync(id);
        
        if (user is null)
        {
            return null;
        }

        await _userRepository.DeleteByIdAsync(id);
        return true;
    }
}