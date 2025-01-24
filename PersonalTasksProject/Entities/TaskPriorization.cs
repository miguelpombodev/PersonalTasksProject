using System.ComponentModel.DataAnnotations;

namespace PersonalTasksProject.Entities;

public class TaskPriorization
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
    public DateTime UpdatedAt { get; set; } = DateTime.Now.ToUniversalTime();
    
    public IEnumerable<UserTask> Tasks { get; set; } = new List<UserTask>();
}