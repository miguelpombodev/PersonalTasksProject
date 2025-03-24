using PersonalTasksProject.DTOs.Database;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Helpers;

namespace PersonalTasksProject.Business.Interfaces;

public interface ITasksService
{
    Task<ServiceResult<UserTask>> CreateUserTaskAsync(UserTask task);
    Task<ServiceResult<PaginatedResponse<CreatedUserTasks>>> GetAllUserTaskAsync(Guid userId, PaginationParameters paginationParameters);
    
    Task<ServiceResult<UserTask>> GetUserTaskByIdAsync(Guid userTaskId);
    Task<bool> DeleteUserTaskAsync(Guid taskId);
    
    Task<ServiceResult<IEnumerable<TaskPriorization>>> GetTasksPrioritiesAsync();
}