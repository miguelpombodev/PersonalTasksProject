using System.Linq.Expressions;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Business.Implementations;

public class TasksService : ITasksService
{
    private readonly IUnitOfWork _unitOfWork;

    private Expression<Func<UserTask, bool>> _getTasksByGuidFilter;

    public TasksService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UserTask> CreateUserTaskAsync(UserTask userTask)
    {
        await _unitOfWork.TasksRepository.AddAsync(userTask);
        
        return userTask;
    }

    public async Task<IEnumerable<CreatedUserTasks>> GetAllUserTaskAsync(Guid userId)
    {
        _getTasksByGuidFilter = tasks => tasks.UserId == userId;
        
        var tasks = await _unitOfWork.TasksRepository.GetAllByPropertyAsync(_getTasksByGuidFilter);
        
        return tasks.OrderBy(task => task.TaskPriorizationId).Select(task => new CreatedUserTasks
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            CompletionDate = task.CompletionDate,
            DueDate = task.DueDate,
            Priority = task.TaskPriorizationId
        });
    }

    public async Task<UserTask?> GetUserTaskByIdAsync(Guid userTaskId)
    {
        return await _unitOfWork.TasksRepository.GetByIdAsync(userTaskId); 
    }

    public async Task<bool> DeleteUserTaskAsync(Guid taskId)
    {
        await _unitOfWork.TasksRepository.DeleteByIdAsync(taskId);
        return true;
    }
}