using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalTasksProject.Entities;

public class UserTask : UserTasksBase
{
    public int TaskPriorizationId { get; set; }
    
    public Guid UserId { get; set; }
    
    public TaskPriorization Status { get; set; }
    
    public User User { get; set; } 
}