using PersonalTasksProject.Context;
using Microsoft.EntityFrameworkCore;

using PersonalTasksProject.Entities;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Repositories.Implementations;

public class TasksRepository : BaseRepository<UserTask>, ITasksRepository
{
    private readonly DbSet<TaskPriorization> _tasksPrioritiesSet;
    public TasksRepository(AppDbContext context) : base(context)
    {
        _tasksPrioritiesSet = context.Set<TaskPriorization>();
    }

    public async Task<IEnumerable<TaskPriorization>> GetPrioritiesListAsync()
    {
        var tasksPrioritiesList = await _tasksPrioritiesSet.ToListAsync();
        
        return tasksPrioritiesList;
    }
}