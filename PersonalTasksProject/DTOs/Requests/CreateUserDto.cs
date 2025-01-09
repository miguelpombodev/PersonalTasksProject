using System.ComponentModel.DataAnnotations;

namespace PersonalTasksProject.DTOs.Requests;

public class CreateUserDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    [MinLength(3, ErrorMessage = "Name cannot be less than 3 characters.")]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}