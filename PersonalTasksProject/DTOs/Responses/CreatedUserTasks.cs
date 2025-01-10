using System.Text.Json.Serialization;

namespace PersonalTasksProject.DTOs.Responses;

public record CreatedUserTasks
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    
    [JsonPropertyName("due_date")]
    public DateOnly DueDate { get; init; }
    public int Priority { get; init; }
    
    [JsonPropertyName("completion_date")]
    public DateOnly? CompletionDate { get; init; }
    
};