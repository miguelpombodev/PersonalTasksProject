using MailKit.Net.Smtp;
using MimeKit;
using PersonalTasksProject.Configuration;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Providers;

public sealed class SmtpEmailProvider
{
    private readonly IAppConfiguration _appConfiguration;
    public SmtpEmailProvider(IAppConfiguration appConfiguration)
    {
       _appConfiguration = appConfiguration;
    }
    
    public async Task<string> SendEmail(string to, string subject, string body)
    {
        var message = _createMessage(to, subject, body);
        
        using var client = new SmtpClient();
        
        try
        {
            await client.ConnectAsync(_appConfiguration.Smtp.Host, _appConfiguration.Smtp.Port, false);
            await client.AuthenticateAsync(_appConfiguration.Smtp.Username, _appConfiguration.Smtp.Password);
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