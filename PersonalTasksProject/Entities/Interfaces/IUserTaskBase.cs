namespace PersonalTasksProject.Entities;

public interface IUserTaskBase : IBaseEntity
{
    string Title { get; }
    string Description { get; }
    DateOnly DueDate { get; }
    DateOnly? CompletionDate { get; }
}