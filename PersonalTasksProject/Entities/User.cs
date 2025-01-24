using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalTasksProject.Entities;

[Table("users")]
public class User : BaseIdentity
{
    [Required]
    [StringLength(100)]
    [MinLength(3)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    public string? AvatarUrl { get; set; } = null;
    
}