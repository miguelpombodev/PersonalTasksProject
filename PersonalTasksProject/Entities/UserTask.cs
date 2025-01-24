using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalTasksProject.Entities;

[Table("user_tasks")]
public class UserTask : UserTasksBase
{
    [Required]
    public int TaskPriorizationId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    public TaskPriorization Status { get; set; }
    
    public User User { get; set; } 
}