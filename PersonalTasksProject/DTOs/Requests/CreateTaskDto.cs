using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PersonalTasksProject.DTOs.Requests;

public class CreateTaskDto : IRequestDto
{

  [JsonIgnore]
  public Guid? UserId { get; set; } = null;

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
  [JsonPropertyName("due_date")]
  public DateOnly DueDate { get; set; }

  [Required]
  [JsonPropertyName("priority")]
  public int TaskPriorizationId { get; set; }

  [DataType(DataType.Date)]
  [JsonPropertyName("completion_date")]
  public DateOnly? CompletionDate { get; set; } = null;
}