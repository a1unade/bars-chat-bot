using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NotifyHub.NotificationService.Application.Interfaces;
using NotifyHub.NotificationService.Infrastructure.Options;

namespace NotifyHub.NotificationService.Infrastructure.Services;

/// <summary>
/// Сервис для отправки сообщений на почту
/// </summary>
public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
{
    /// <summary>
    /// Настройки SMTP-клиента
    /// </summary>
    private readonly EmailOptions _options = configuration.GetSection("EmailSettings").Get<EmailOptions>()!;
    
    private readonly ILogger<IEmailService> _logger = logger;

    public async Task SendEmailAsync(string email, string subject, string messageBody)
    {
        try
        {
            using var smtpClient = new SmtpClient();
            
            await smtpClient.ConnectAsync(_options.Host, _options.Port, true);
            await smtpClient.AuthenticateAsync(_options.EmailAddress, _options.Password);
            await smtpClient.SendAsync(GenerateMessage(email, subject, messageBody));
            await smtpClient.DisconnectAsync(true);
            
            _logger.LogInformation("Mail successfully sent to: {0}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to send mail message: {0}", ex.Message);
        }
    }

    public MimeMessage GenerateMessage(string email, string subject, string messageBody)
    {
        return new MimeMessage
        {
            From = { new MailboxAddress("NotifyHub", _options.EmailAddress) },
            To = { new MailboxAddress("", email) },
            Subject = subject,
            Body = new BodyBuilder { HtmlBody = messageBody }.ToMessageBody()
        };
    }
}