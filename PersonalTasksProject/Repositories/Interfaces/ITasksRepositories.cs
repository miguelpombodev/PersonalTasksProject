using PersonalTasksProject.DTOs.Database;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Repositories.Interfaces;

public interface ITasksRepository : IBaseRepository<UserTask> 
{
    public Task<IEnumerable<TaskPriorization>> GetPrioritiesListAsync();
    public Task<IEnumerable<UserTasksWithNamedPriority>> GetAllTasksAsync(Guid userId);
}