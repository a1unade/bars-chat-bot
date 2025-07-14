using System.ComponentModel;

namespace NotifyHub.Domain.Common.Enums;

/// <summary>
/// Периодичность уведомлений
/// </summary>
public enum NotificationFrequency
{
    [Description("Каждый час")]
    Hourly = 0,
    
    [Description("Каждый день")]
    Daily = 1,
    
    [Description("Каждую неделю")]
    Weekly = 2,
    
    [Description("Каждый месяц")]
    Monthly = 3,
    
    [Description("Каждый год")]
    Yearly = 4
}