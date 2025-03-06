using PersonalTasksProject.Configuration.Settings;

namespace PersonalTasksProject.Configuration;

public class AppConfiguration : IAppConfiguration
{
    private static AppConfiguration _instance;
    
    private static readonly object _lock = new object();

    public static AppConfiguration Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new AppConfiguration();
                }
            }

            return _instance;
        }
    }

    public string PostgresConnection { get; set; }
    public SMTPConfigurationSettings Smtp { get; set; } = new ();
    public JwtConfigurationSettings Jwt { get; set; }= new ();
    public AwsConfigurationSettings Aws { get; set; }= new ();
    public Dictionary<string, string> EmailBodies { get; set; }
}