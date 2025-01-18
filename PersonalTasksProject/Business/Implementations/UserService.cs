using Microsoft.AspNetCore.Authorization;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Extensions;
using PersonalTasksProject.Repositories.Implementations;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Business.Implementations;

public class UserService: IUserService
{
    private readonly UnitOfWork _unitOfWork;

    public UserService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [Authorize]
    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        User result = await _unitOfWork.UserRepository.GetByIdAsync(id);

        if (result is null)
        {
            return null;
        }

        return result;
    }

    [Authorize]
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        User result = await _unitOfWork.UserRepository.GetByPropertyAsync(user => user.Email == email);
        
        if (result is null)
        {
            return null;
        }

        return result;
        
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _unitOfWork.UserRepository.AddAsync(user);
        return user;
    }

    [Authorize]
    public Task<User> UpdateUserAsync(User user, CreatedUserResponseDto body)
    {
        user.UpdateFromDto(body);
        _unitOfWork.UserRepository.UpdateAsync(user);
        return Task.FromResult(user);
    }
    

    [Authorize]
    public async Task<bool?> DeleteUserAsync(Guid id)
    {
        User user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        
        if (user is null)
        {
            return null;
        }

        await _unitOfWork.UserRepository.DeleteByIdAsync(id);
        return true;
    }
}