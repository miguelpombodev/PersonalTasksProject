using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalTasksProject.Entities;

[Table("user_tasks")]
public class UserTask : BaseIdentity
{
    [Required]
    [StringLength(50)]
    [MinLength(3)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(200)]
    [MinLength(3)]
    public string Description { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Required]
    public int TaskPriorizationId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    public DateTime? CompletionDate { get; set; }
    
    public TaskPriorization Status { get; set; }
    
    public User User { get; set; }
}