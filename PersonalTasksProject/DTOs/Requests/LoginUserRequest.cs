using System.ComponentModel.DataAnnotations;

namespace PersonalTasksProject.DTOs.Requests;

public class LoginUserRequest: IRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public required string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password, ErrorMessage = "Password must be sent.")]
    public required string Password { get; set; }
}