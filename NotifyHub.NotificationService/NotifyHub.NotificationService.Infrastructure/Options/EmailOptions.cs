namespace NotifyHub.NotificationService.Infrastructure.Options;

/// <summary>
/// Настройки для почтового клиента
/// </summary>
public class EmailOptions
{
    /// <summary>
    /// Хост
    /// </summary>
    public required string Host { get; set; }
    
    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; set; }
    
    /// <summary>
    /// Адрес
    /// </summary>
    public required string EmailAddress { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public required string Password { get; set; }
}