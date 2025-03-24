using System.Linq.Expressions;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Database;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Helpers;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Business.Implementations;

public class TasksService : ITasksService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public TasksService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ServiceResult<UserTask>> CreateUserTaskAsync(UserTask userTask)
    {
        await _unitOfWork.TasksRepository.AddAsync(userTask);
        
        _unitOfWork.Commit();
        
        return ServiceResult<UserTask>.Success(userTask);
    }

    public async Task<ServiceResult<PaginatedResponse<CreatedUserTasks>>> GetAllUserTaskAsync(Guid userId, PaginationParameters paginationParameters)
    {
        var resultTasks = await _unitOfWork.TasksRepository.GetAllTasksAsync(userId, paginationParameters);
        
        var tasks = CreatedUserTasks.ToList(resultTasks.Data);

        var paginatedTasksResponse = new PaginatedResponse<CreatedUserTasks>(resultTasks.Pagination, tasks);
        
        return ServiceResult<PaginatedResponse<CreatedUserTasks>>.Success(paginatedTasksResponse);
    }

    public async Task<ServiceResult<UserTask>> GetUserTaskByIdAsync(Guid userTaskId)
    {
        var userTaskResult = await _unitOfWork.TasksRepository.GetByIdAsync(userTaskId);

        if (userTaskResult == null)
        {
            return ServiceResult<UserTask>.Failure("Task not found");
        }

        return ServiceResult<UserTask>.Success(userTaskResult);
    }

    public async Task<bool> DeleteUserTaskAsync(Guid taskId)
    {
        await _unitOfWork.TasksRepository.DeleteByIdAsync(taskId);
        return true;
    }

    public async Task<ServiceResult<IEnumerable<TaskPriorization>>> GetTasksPrioritiesAsync()
    {
        var tasksPriorizationList = await _unitOfWork.TasksRepository.GetPrioritiesListAsync();
        
        return ServiceResult<IEnumerable<TaskPriorization>>.Success(tasksPriorizationList);
    }
}