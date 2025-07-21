using MediatR;
using AutoMapper;
using NotifyHub.Abstractions.DTOs;
using NotifyHub.Domain.Events;
using NotifyHub.Domain.Entities;
using NotifyHub.Application.Requests.Notification;
using NotifyHub.Application.Interfaces.Repositories;

namespace NotifyHub.Application.Features.Mutations;

[ExtendObjectType("Mutation")]
public class NotificationMutation(IMapper mapper, IMediator mediator) : BaseMutation
{
    [GraphQLDescription("Создание уведомления")]
    public async Task<NotificationDto> CreateNotificationAsync(
        [Service] IGenericRepository<Notification> notificationRepository,
        [Service] IGenericRepository<User> userRepository,
        CreateNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new ArgumentException("Пользователь с указанным ID не найден");

        var notification = mapper.Map<Notification>(request);
        notification.User = user;

        var savedEntity = await Add(notificationRepository, notification, cancellationToken);
        var savedDto = mapper.Map<NotificationDto>(savedEntity);
        
        await mediator.Publish(new NotificationCreatedDomainEvent(savedDto), cancellationToken);

        return savedDto;
    }
    
    [GraphQLDescription("Обновление уведомления по ID")]
    public async Task<NotificationDto> UpdateNotificationAsync(
        [Service] IGenericRepository<Notification> notificationRepository,
        [Service] IGenericRepository<User> userRepository,
        UpdateNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (notification is null)
            throw new ArgumentException("Уведомление с указанным ID не найдено");
        
        if (request.Title != null)
            notification.Title = request.Title;
        if (request.Description != null)
            notification.Description = request.Description;
        if (request.Type.HasValue)
            notification.Type = request.Type.Value;
        if (request.Frequency.HasValue)
            notification.Frequency = request.Frequency;
        if (request.ScheduledAt.HasValue)
            notification.ScheduledAt = request.ScheduledAt.Value;

        var updatedEntity = await notificationRepository.UpdateAsync(notification.Id, notification, cancellationToken);
        var savedDto = mapper.Map<NotificationDto>(updatedEntity);
        
        await mediator.Publish(new NotificationUpdatedDomainEvent(savedDto), cancellationToken);

        return savedDto;
    }
    
    [GraphQLDescription("Удаление уведомления по ID")]
    public async Task<bool> DeleteNotificationAsync(
        [Service] IGenericRepository<Notification> notificationRepository,
        Guid id,
        CancellationToken cancellationToken)
    {
        await notificationRepository.RemoveByIdAsync(id, cancellationToken);
        await mediator.Publish(new NotificationDeletedDomainEvent(id), cancellationToken);

        return true;
    }
}
