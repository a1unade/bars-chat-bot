using FluentValidation;
using NotifyHub.Abstractions.Enums;
using NotifyHub.Application.Requests.Notification;

namespace NotifyHub.Application.Validators;

public class UpdateNotificationRequestValidator : AbstractValidator<UpdateNotificationRequest>
{
    public UpdateNotificationRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id уведомления обязателен.");

        When(x => x.Title is not null, () =>
        {
            RuleFor(x => x.Title!)
                .NotEmpty().WithMessage("Title не может быть пустым.")
                .MaximumLength(200).WithMessage("Title не должен превышать 200 символов.");
        });

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description!)
                .MaximumLength(1000).WithMessage("Description не должен превышать 1000 символов.");
        });

        When(x => x.ScheduledAt.HasValue, () =>
        {
            RuleFor(x => x.ScheduledAt!.Value)
                .Must(BeInTheFuture).WithMessage("ScheduledAt должен быть в будущем.");
        });

        // Проверка связки Type и Frequency
        When(x => x.Type == NotificationType.Recurring, () =>
        {
            RuleFor(x => x.Frequency)
                .NotNull().WithMessage("Frequency обязателен для периодических уведомлений.");
        });

        When(x => x.Type == NotificationType.OneTime, () =>
        {
            RuleFor(x => x.Frequency)
                .Null().WithMessage("Frequency должен быть null для разовых уведомлений.");
        });
    }

    private bool BeInTheFuture(DateTime dateTime)
    {
        return dateTime > DateTime.UtcNow;
    }
}