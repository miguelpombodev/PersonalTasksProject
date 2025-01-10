using System.ComponentModel.DataAnnotations;

namespace PersonalTasksProject.DTOs.Requests;

public class CreateTaskDto
{
    [Required]
    [StringLength(50, ErrorMessage = "Title must be between 3 and 50 characters")]
    [MinLength(3, ErrorMessage = "Title must be between 3 and 50 characters")]
    public string Title { get; set; }
    
    [Required]
    [StringLength(200, ErrorMessage = "Description must be between 3 and 200 characters")]
    [MinLength(3, ErrorMessage = "Description must be between 3 and 200 characters")]
    public string Description { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateOnly DueDate { get; set; }
    
    [Required]
    public int Priority { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? CompletionDate { get; set; }
}