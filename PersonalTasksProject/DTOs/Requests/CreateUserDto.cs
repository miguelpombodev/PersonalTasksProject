using System.ComponentModel.DataAnnotations;

namespace PersonalTasksProject.DTOs.Requests;

public class CreateUserDto: IRequestDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    [MinLength(3, ErrorMessage = "Name cannot be less than 3 characters.")]
    public required string Name { get; set;}
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; set;}
    
    [Required]
    [MinLength(3, ErrorMessage = "Password cannot be less than 3 characters.")]
    public required string Password { get; set;}
}