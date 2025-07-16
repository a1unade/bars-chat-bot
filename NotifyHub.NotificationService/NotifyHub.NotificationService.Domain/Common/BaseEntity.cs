namespace NotifyHub.NotificationService.Domain.Common;

public abstract class BaseEntity
{
    /// <summary>
    /// ID сущности
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Когда была создана сущность
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Когда была обновлена сущность
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}