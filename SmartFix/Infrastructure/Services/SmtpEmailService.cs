using MailKit.Net.Smtp;
using MimeKit;
using SmartFix.Domain.Abstractions;

namespace SmartFix.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IConfiguration config, ILogger<SmtpEmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        try
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("SmartFix Service", _config["Email:From"]));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
          
            await client.ConnectAsync(
                _config["Email:Host"], 
                int.Parse(_config["Email:Port"]), 
                MailKit.Security.SecureSocketOptions.StartTls, 
                cancellationToken
            );
            client.AuthenticationMechanisms.Remove("XOAUTH2"); 
            await client.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"], cancellationToken);
            
            await client.SendAsync(emailMessage, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
            
            _logger.LogInformation($"Email sent to {to}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send email to {to}");
        }
    }
}