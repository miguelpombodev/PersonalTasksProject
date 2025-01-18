using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Extensions;
using PersonalTasksProject.Helpers;
using PersonalTasksProject.Repositories.Implementations;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Business.Implementations;

public class UserService: IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<User>> GetUserByIdAsync(Guid id)
    {
        var result = await _unitOfWork.UserRepository.GetByIdAsync(id);

        if (result is null)
        {
            return ServiceResult<User>.Failure("User not found");
        }

        return ServiceResult<User>.Success(result);
    }

    public async Task<ServiceResult<User>> GetUserByEmailAsync(string email)
    {
        var result = await _unitOfWork.UserRepository.GetByPropertyAsync(user => user.Email == email);
        
        if (result is null)
        {
            return ServiceResult<User>.Failure("Please check your email address or password  not found");
        }

        return ServiceResult<User>.Success(result);
        
    }

    public async Task<ServiceResult<User>> CreateUserAsync(User user)
    {
        await _unitOfWork.UserRepository.AddAsync(user);

        _unitOfWork.Commit();
        
        return ServiceResult<User>.Success(user);
    }

    public async Task<ServiceResult<User>> UpdateUserAsync(User user, CreatedUserResponseDto body)
    {
        user.UpdateFromDto(body);
        _unitOfWork.UserRepository.UpdateAsync(user);
        
        _unitOfWork.Commit();

        return ServiceResult<User>.Success(user);
    }
    

    public async Task<bool?> DeleteUserAsync(Guid id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        
        if (user is null)
        {
            return null;
        }

        await _unitOfWork.UserRepository.DeleteByIdAsync(id);
        
        _unitOfWork.Commit();

        return true;
    }
}