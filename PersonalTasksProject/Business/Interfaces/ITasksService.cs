using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Helpers;

namespace PersonalTasksProject.Business.Interfaces;

public interface ITasksService
{
    Task<ServiceResult<UserTask>> CreateUserTaskAsync(UserTask task);
    Task<ServiceResult<IEnumerable<CreatedUserTasks>>> GetAllUserTaskAsync(Guid userId);
    
    Task<ServiceResult<UserTask>> GetUserTaskByIdAsync(Guid userTaskId);
    Task<bool> DeleteUserTaskAsync(Guid taskId);
}