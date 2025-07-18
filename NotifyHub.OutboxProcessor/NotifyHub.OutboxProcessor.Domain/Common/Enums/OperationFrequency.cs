using System.ComponentModel;

namespace NotifyHub.OutboxProcessor.Domain.Common.Enums;

/// <summary>
/// Периодичность отправки
/// </summary>
public enum OperationFrequency
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