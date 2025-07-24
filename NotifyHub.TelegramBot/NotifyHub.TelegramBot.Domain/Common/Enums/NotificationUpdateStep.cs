using System.ComponentModel;

namespace NotifyHub.TelegramBot.Domain.Common.Enums;

/// <summary>
/// Шаги при обновлении уведомления
/// </summary>
public enum NotificationUpdateStep
{
    [Description("ID уведомления")]
    AskId,
    
    [Description("Заголовок")]
    AskTitle,
    
    [Description("Описание")]
    AskDescription,
    
    [Description("Тип уведомления")]
    AskType,
    
    [Description("Частота отправки")]
    AskFrequency,
    
    [Description("Во сколько запланирована отправка")]
    AskScheduledAt,
    
    [Description("Подтверждение изменений")]
    Confirm
}