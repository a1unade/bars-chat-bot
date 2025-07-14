using System.ComponentModel;

namespace NotifyHub.Domain.Common.Enums;

/// <summary>
/// Тип уведомления
/// </summary>
public enum NotificationType
{
    [Description("Одноразовое")]
    OneTime = 0,
    
    [Description("Периодическое")]
    Recurring = 1
}