using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Repositories.Interfaces;

public interface ITasksRepository : IBaseRepository<UserTask> 
{
    public Task<IEnumerable<TaskPriorization>> GetPrioritiesListAsync();
}