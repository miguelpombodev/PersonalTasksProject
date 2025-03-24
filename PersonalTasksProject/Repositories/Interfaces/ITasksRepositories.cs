using PersonalTasksProject.DTOs.Database;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Helpers;

namespace PersonalTasksProject.Repositories.Interfaces;

public interface ITasksRepository : IBaseRepository<UserTask> 
{
    public Task<IEnumerable<TaskPriorization>> GetPrioritiesListAsync();
    public Task<PagedList<UserTasksWithNamedPriority>> GetAllTasksAsync(Guid userId, PaginationParameters pagination);
}