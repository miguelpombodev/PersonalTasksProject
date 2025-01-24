using System.ComponentModel.DataAnnotations;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.DTOs.Database;

public class UserTasksWithNamedPriority : UserTasksBase
{
    [Required]
    public string Priority { get; set; }
}