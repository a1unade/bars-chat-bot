using System.ComponentModel;

namespace NotifyHub.TelegramBot.Domain.Common.Enums;

/// <summary>
/// Шаги создания уведомления
/// </summary>
public enum NotificationCreationStep
{
    [Description("Заголовок")]
    Title,
    
    [Description("Описание")]
    Description,
    
    [Description("Тип уведомления")]
    Type,
    
    [Description("Во сколько запланирована отправка")]
    ScheduledAt,
    
    [Description("Частота отправки")]
    Frequency,
    
    [Description("Подтверждение")]
    Confirm
}