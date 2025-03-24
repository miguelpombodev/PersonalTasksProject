using System.ComponentModel.DataAnnotations;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.DTOs.Database;

public class UserTasksWithNamedPriority : UserTasksBase, IUserTasksWithNamedPriority
{
    [Required]
    public string Priority { get; set; }
}