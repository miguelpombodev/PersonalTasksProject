namespace PersonalTasksProject.DTOs.Responses;


public record CreatedUserResponseDto(string Name, string Email, string Avatar)
{
    public string Name { get; set; } = Name;
    public string Email { get; set; } = Email;
    public string Avatar { get; set; } = Avatar;
};