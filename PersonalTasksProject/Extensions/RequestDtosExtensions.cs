using PersonalTasksProject.DTOs;

namespace PersonalTasksProject.Extensions;

public static class RequestDtosExtensions
{
    public static bool AreAllPropertiesNullables<T>(this T dto) where T : IRequestDto
    {
        return dto.GetType().GetProperties().Where(p => p.CanRead).All(p => p.GetValue(dto) == null);
    }
}