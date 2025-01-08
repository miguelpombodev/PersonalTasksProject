using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalTasksProject.Entities;

[Table("users")]
public class User : BaseIdentity
{
    [Required]
    [StringLength(100)]
    [MinLength(3)]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    public string AvatarUrl { get; set; } = "default.png";
    
}