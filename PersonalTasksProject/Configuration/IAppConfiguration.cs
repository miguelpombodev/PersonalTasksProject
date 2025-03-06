using PersonalTasksProject.Configuration.Settings;

namespace PersonalTasksProject.Configuration;

public interface IAppConfiguration
{
    string PostgresConnection { get; set;}
    SMTPConfigurationSettings Smtp { get; set;}
    JwtConfigurationSettings Jwt { get; set;}
    AwsConfigurationSettings Aws { get; set;}
    Dictionary<string, string> EmailBodies { get; set;}
}