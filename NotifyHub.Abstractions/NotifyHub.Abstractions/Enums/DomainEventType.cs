using System.ComponentModel;

namespace NotifyHub.Abstractions.Enums;

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