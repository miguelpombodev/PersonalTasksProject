using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Extensions;

public static class UserExtensions
{
    public static void UpdateFromDto(this User user, CreatedUserResponseDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Name))
            user.Name = dto.Name;
            
        if (!string.IsNullOrEmpty(dto.Email))
            user.Email = dto.Email;

        user.UpdatedAt = DateTime.UtcNow;
    }
}