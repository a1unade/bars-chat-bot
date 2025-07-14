using System.ComponentModel;

namespace NotifyHub.Domain.Common.Enums;

/// <summary>
/// Статус операции
/// </summary>
public enum OperationStatus
{
    [Description("Создана")]
    Created,
    
    [Description("Выполняется")]
    InProgress,
    
    [Description("Завершена")]
    Completed,
    
    [Description("Отменена")]
    Cancelled
}