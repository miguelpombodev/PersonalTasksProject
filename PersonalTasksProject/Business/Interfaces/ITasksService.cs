using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Business.Interfaces;

public interface ITasksService
{
    Task<UserTask> CreateUserTaskAsync(UserTask task);
    Task<IEnumerable<CreatedUserTasks>> GetAllUserTaskAsync(Guid userId);
}