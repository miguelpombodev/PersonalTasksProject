using System.ComponentModel.DataAnnotations;

namespace PersonalTasksProject.Entities;

public class BaseIdentity
{
    public BaseIdentity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    [Key]
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}