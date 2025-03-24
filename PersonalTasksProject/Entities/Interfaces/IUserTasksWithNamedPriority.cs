namespace PersonalTasksProject.Entities;

public interface IUserTasksWithNamedPriority : IUserTaskBase
{
    string Priority { get; }
}