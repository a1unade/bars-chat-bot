using NotifyHub.Abstractions.Enums;

namespace NotifyHub.Application.Helpers;

public static class NotificationHelper
{
    /// <summary>
    /// Получение нового времени для отправки
    /// </summary>
    /// <param name="frequency">Частота отправки</param>
    /// <returns>Время отправки (когда необходимо отправить сообщение)</returns>
    public static DateTime GetNewScheduledAt(NotificationFrequency frequency) =>
        frequency switch
        {
            NotificationFrequency.Hourly => DateTime.UtcNow.AddHours(1),
            NotificationFrequency.Daily => DateTime.UtcNow.AddDays(1),
            NotificationFrequency.Weekly => DateTime.UtcNow.AddDays(7),
            NotificationFrequency.Monthly => DateTime.UtcNow.AddMonths(1),
            NotificationFrequency.Yearly => DateTime.UtcNow.AddYears(1),
            _ => DateTime.UtcNow
        };
}