using PersonalTasksProject.Context;
using Microsoft.EntityFrameworkCore;
using PersonalTasksProject.DTOs.Database;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Helpers;
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

    public async Task<PagedList<UserTasksWithNamedPriority>> GetAllTasksAsync(Guid userId,
        PaginationParameters pagination)
    {
        var tasksQuery = _dbSet.Join(_tasksPrioritiesSet, tasks => tasks.TaskPriorizationId,
            priority => priority.Id, (tasks, priority) => new
            {
                Id = tasks.Id,
                Title = tasks.Title,
                Description = tasks.Description,
                DueDate = tasks.DueDate,
                Priority = priority.Name,
                PriorityId = priority.Id,
                CompletionDate = tasks.CompletionDate,
                UserId = userId
            }).Where(t => t.UserId == userId).OrderBy(task => task.PriorityId).OrderBy(tasks => tasks.Priority).Select(
            task =>
                new UserTasksWithNamedPriority
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    Priority = task.Priority,
                    CompletionDate = task.CompletionDate,
                }).AsQueryable();
        
        var tasksList = await PagedList<UserTasksWithNamedPriority>.ToPagedList(tasksQuery, pagination.PageNumber, pagination.PageSize);

        return tasksList;
    }
}