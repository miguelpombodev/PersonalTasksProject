using System.Text.Json.Serialization;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.DTOs.Responses;

public class CreatedUserTasks
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    
    [JsonPropertyName("due_date")]
    public DateOnly DueDate { get; init; }
    public string Priority { get; init; }
    
    [JsonPropertyName("completion_date")]
    public DateOnly? CompletionDate { get; init; }

    public CreatedUserTasks(Guid id, string title, string description, DateOnly dueDate, DateOnly? completionDate, string priority)
    {
        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
        CompletionDate = completionDate;
        Priority = priority;
    }

    public static List<CreatedUserTasks> ToList<T>(List<T> list) where T : IUserTasksWithNamedPriority
    {
        var entityList = list.Select(task => new CreatedUserTasks(
            task.Id, 
            task.Title, 
            task.Description, 
            task.DueDate, 
            task.CompletionDate, 
            task.Priority
            )
        ).ToList();
       
        
        return entityList;
    }
    
};