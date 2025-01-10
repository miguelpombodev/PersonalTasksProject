using System.Linq.Expressions;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Business.Implementations;

public class TasksService : ITasksService
{
    private readonly ITasksRepositories _tasksRepository;

    private Expression<Func<UserTask, bool>> _getTasksByGuidFilter;

    public TasksService(ITasksRepositories tasksRepository)
    {
        _tasksRepository = tasksRepository;
    }
    
    public async Task<UserTask> CreateUserTaskAsync(UserTask userTask)
    {
        await _tasksRepository.AddAsync(userTask);
        
        return userTask;
    }

    public async Task<IEnumerable<CreatedUserTasks>> GetAllUserTaskAsync(Guid userId)
    {
        _getTasksByGuidFilter = tasks => tasks.UserId == userId;
        
        var tasks = await _tasksRepository.GetAllByPropertyAsync(_getTasksByGuidFilter);
        
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
        return await _tasksRepository.GetByIdAsync(userTaskId); 
    }

    public async Task<bool> DeleteUserTaskAsync(Guid taskId)
    {
        await _tasksRepository.DeleteByIdAsync(taskId);
        return true;
    }
}