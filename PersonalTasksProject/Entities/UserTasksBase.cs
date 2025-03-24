using System.ComponentModel.DataAnnotations;

namespace PersonalTasksProject.Entities;

public class UserTasksBase : BaseIdentity, IUserTaskBase
{
    [Required]
    [StringLength(50)]
    [MinLength(3)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    [MinLength(3)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public DateOnly DueDate { get; set; }

    public DateOnly? CompletionDate { get; set; }
}