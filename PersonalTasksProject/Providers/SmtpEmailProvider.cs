using MailKit.Net.Smtp;
using MimeKit;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Providers;

public sealed class SmtpEmailProvider
{
    private readonly SmtpServerConfig _smtpServer;
    public SmtpEmailProvider(IConfiguration configuration)
    {
        var smtpServerConfig = new SmtpServerConfig();
        configuration.GetSection("SmtpMailTrap").Bind(smtpServerConfig);
        _smtpServer = smtpServerConfig;
    }
    public async Task<string> SendEmail(string to, string subject, string body)
    {
        var message = _createMessage(to, subject, body);
        
        using var client = new SmtpClient();
        
        try
        {
            await client.ConnectAsync(_smtpServer.Host, _smtpServer.Port, false);
            await client.AuthenticateAsync(_smtpServer.Username, _smtpServer.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            
            return "success";
        }
        catch ( Exception e )
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private MimeMessage _createMessage(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Foodie", "no-respond@foodie.com"));
        message.To.Add(new MailboxAddress("User", to));
        message.Subject = subject;


        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };

        message.Body = bodyBuilder.ToMessageBody();
        
        return message;
    }
}