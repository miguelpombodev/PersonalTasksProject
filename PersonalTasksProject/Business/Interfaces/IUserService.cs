using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Business.Interfaces;

public interface IUserService
{
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user, CreatedUserResponseDto body);
    Task<bool?> DeleteUserAsync(Guid id);
}