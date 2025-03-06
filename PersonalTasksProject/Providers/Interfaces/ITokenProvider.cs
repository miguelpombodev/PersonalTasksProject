using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Providers.Interfaces;

public interface ITokenProvider
{
    string CreateToken(User user);
    string DecodeToken(string token);
}