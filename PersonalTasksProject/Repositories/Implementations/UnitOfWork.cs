using PersonalTasksProject.Context;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private IUserRepository _userRepository;
    private ITasksRepository _tasksRepository;
    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IUserRepository UserRepository
    {
        get
        {
            return _userRepository =  _userRepository ?? new UserRepository(_context);
        }
    }

    public ITasksRepository TasksRepository
    {
        get
        {
            return _tasksRepository = _tasksRepository ?? new TasksRepository(_context);
        }
    }

    public async void Commit()
    {
        await _context.SaveChangesAsync();
    }

    public async void Dispose()
    {
        await _context.DisposeAsync();
    }
}