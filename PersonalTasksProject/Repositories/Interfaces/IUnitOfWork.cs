namespace PersonalTasksProject.Repositories.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    ITasksRepository TasksRepository { get; } 
    void Commit();
}