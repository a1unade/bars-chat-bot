using System.ComponentModel;

namespace NotifyHub.OutboxProcessor.Domain.Common.Enums;

/// <summary>
/// Тип операции
/// </summary>
public enum OperationType
{
    [Description("Одноразовое")]
    OneTime = 0,
    
    [Description("Периодическое")]
    Recurring = 1
}