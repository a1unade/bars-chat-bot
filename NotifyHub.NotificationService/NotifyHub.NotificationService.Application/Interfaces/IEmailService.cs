using MimeKit;

namespace NotifyHub.NotificationService.Application.Interfaces;

public interface IEmailService
{
    /// <summary>
    /// Отправка письма
    /// </summary>
    /// <param name="email">Почта пользователя</param>
    /// <param name="subject">Заголовок сообщения</param>
    /// <param name="messageBody">Сообщение</param>
    Task SendEmailAsync(string email, string subject, string messageBody);

    /// <summary>
    /// Создание письма перед отправкой
    /// </summary>
    /// <param name="email">Почта получателя</param>
    /// <param name="subject">Тема письма</param>
    /// <param name="messageBody">Текст сообщения</param>
    /// <returns>Объект MimeMessage для отправки на почту</returns>
    MimeMessage GenerateMessage(string email, string subject, string messageBody);
}