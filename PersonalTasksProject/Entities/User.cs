using System.ComponentModel.DataAnnotations;
using PersonalTasksProject.Providers;

namespace PersonalTasksProject.Entities;

public class User : BaseIdentity
{
    private string _hashedPassword;
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Password {
        get => _hashedPassword;
        set => _hashedPassword = _validateIfStringIsAlreadyHashedPassword(value);
    }
    
    public string? AvatarUrl { get; set; } = null;
    
    public IEnumerable<UserTask>? Tasks { get; } = new List<UserTask>();

    private string _validateIfStringIsAlreadyHashedPassword(string value)
    {
        if (value.StartsWith("$2") &&
            value.Length == 60)
        {
            return value;
        }

        return DataEncryptionProvider.Hash(value);
    }
}