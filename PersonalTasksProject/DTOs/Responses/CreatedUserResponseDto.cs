using System.ComponentModel.DataAnnotations;

namespace PersonalTasksProject.DTOs.Responses;


public record CreatedUserResponseDto(string Name, string Email, string Avatar)
{
    [Required]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    [MinLength(3, ErrorMessage = "Name cannot be less than 3 characters.")]
    public string Name { get; set; } = Name;
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = Email;
    
    [Required]
    public string Avatar { get; set; } = Avatar;
};