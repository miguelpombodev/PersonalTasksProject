using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Helpers;

namespace PersonalTasksProject.Business.Interfaces;

public interface IUserService
{
    Task<ServiceResult<User>> GetUserByIdAsync(Guid id);
    Task<ServiceResult<User>> GetUserByEmailAsync(string email);
    Task<ServiceResult<User>> CreateUserAsync(User user);
    Task<ServiceResult<User>> UpdateUserAsync(User user, CreatedUserResponseDto body);
    Task<bool?> DeleteUserAsync(Guid id);
}