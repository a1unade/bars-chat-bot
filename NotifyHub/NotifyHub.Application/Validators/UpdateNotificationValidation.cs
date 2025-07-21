using FluentValidation;
using NotifyHub.Application.Requests.Notification;
using NotifyHub.Domain.Common.Enums;

namespace NotifyHub.Application.Validators;

public class UpdateNotificationValidation : AbstractValidator<UpdateNotificationRequest>
{
    public UpdateNotificationValidation()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID уведомления не может быть пустым!")
            .Must(BeAValidGuid)
            .WithMessage("ID уведомления должен быть корректным GUID!");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Название уведомления не может быть пустым!")
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithMessage("Название уведомления не должно быть пустым или состоять из пробелов!")
            .Length(1, 100)
            .WithMessage("Название уведомления должно быть от 1 до 100 символов!")
            .When(x => x.Title != null);

        RuleFor(x => x.Description)
            .Must(desc => !string.IsNullOrWhiteSpace(desc))
            .WithMessage("Описание не должно состоять из пробелов!")
            .When(x => x.Description != null);

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Указан недопустимый тип уведомления!")
            .When(x => x.Type.HasValue);

        RuleFor(x => x.Frequency)
            .IsInEnum()
            .WithMessage("Указана недопустимая частота уведомления!")
            .When(x => x.Frequency.HasValue);

        RuleFor(x => x.ScheduledAt)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Дата отправки должна быть запланирована на будущее!")
            .When(x => x.ScheduledAt.HasValue);
    }

    private bool BeAValidGuid(Guid guid) => Guid.Equals(guid, Guid.Empty);
}
