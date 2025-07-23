using FluentValidation;
using NotifyHub.Abstractions.Enums;
using NotifyHub.Application.Requests.Notification;

namespace NotifyHub.Application.Validators;

public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
{
    public CreateNotificationRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId обязателен.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title обязателен.")
            .MaximumLength(200).WithMessage("Title не должен превышать 200 символов.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description не должен превышать 1000 символов.");

        RuleFor(x => x.ScheduledAt)
            .Must(BeInTheFuture).WithMessage("ScheduledAt должен быть в будущем.");

        RuleFor(x => x.Frequency)
            .NotNull()
            .When(x => x.Type == NotificationType.Recurring)
            .WithMessage("Frequency обязателен для периодических уведомлений.");

        RuleFor(x => x.Frequency)
            .Null()
            .When(x => x.Type == NotificationType.OneTime)
            .WithMessage("Frequency должен быть null для разовых уведомлений.");
    }

    private bool BeInTheFuture(DateTime dateTime)
    {
        return dateTime > DateTime.UtcNow;
    }
}