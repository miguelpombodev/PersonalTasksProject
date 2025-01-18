using PersonalTasksProject.Context;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Repositories.Implementations;

public class TasksRepository : BaseRepository<UserTask>, ITasksRepository
{
    private readonly AppDbContext _context;
    
    public TasksRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}