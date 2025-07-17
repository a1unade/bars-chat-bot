using System.ComponentModel;

namespace NotifyHub.Domain.Common.Enums;

/// <summary>
/// Статус операции
/// </summary>
public enum OperationStatus
{
    [Description("Создана")]
    Created,

    [Description("В процессе")]
    InProgress,

    [Description("Успешно отправлена")]
    Sent,

    [Description("Ошибка отправки")]
    Failed,

    [Description("Отменена")]
    Cancelled
}