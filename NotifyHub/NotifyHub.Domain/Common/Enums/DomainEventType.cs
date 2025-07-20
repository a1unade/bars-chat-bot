using System.ComponentModel;

namespace NotifyHub.Domain.Common.Enums;

/// <summary>
/// Тип доменного события у сущности
/// </summary>
public enum DomainEventType
{
    [Description("Создание")]
    Created,
    
    [Description("Обновление")]
    Updated,
    
    [Description("Удаление")]
    Deleted
}