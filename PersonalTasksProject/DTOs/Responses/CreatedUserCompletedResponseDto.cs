namespace PersonalTasksProject.DTOs.Responses;

public record CreatedUserCompletedResponseDto(string Name, string Email, string Avatar, Guid Id) : CreatedUserResponseDto(Name, Email, Avatar)
{
    public Guid Id { get; set; } = Id;
};