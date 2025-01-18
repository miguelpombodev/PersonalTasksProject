using PersonalTasksProject.Context;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Repositories.Implementations;

public class TasksRepository : BaseRepository<UserTask>, ITasksRepository
{
    public TasksRepository(AppDbContext context) : base(context)
    {
    }
}