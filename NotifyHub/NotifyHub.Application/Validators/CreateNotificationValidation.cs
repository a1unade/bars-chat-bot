using FluentValidation;
using NotifyHub.Application.Requests.Notification;

namespace NotifyHub.Application.Validators;
public class CreateNotificationValidation : AbstractValidator<CreateNotificationRequest>
{
    public CreateNotificationValidation()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID не может быть пустым!")
            .Must(BeAValidGuid)
            .WithMessage("User ID должен быть корректным GUID!");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Название уведомления не может быть пустым!")
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithMessage("Название уведомления не должно состоять из пробелов!")
            .Length(1, 100)
            .WithMessage("Название уведомления должно быть от 1 до 100 символов!");

        RuleFor(x => x.Description)
            .Must(desc => !string.IsNullOrWhiteSpace(desc))
            .WithMessage("Описание не должно состоять из пробелов!");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Указан недопустимый тип уведомления!");

        RuleFor(x => x.Frequency)
            .IsInEnum()
            .WithMessage("Указана недопустимая частота уведомления!")
            .When(x => x.Frequency.HasValue);

        RuleFor(x => x.ScheduledAt)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Дата отправки должна быть запланирована на будущее!");
    }

    private bool BeAValidGuid(Guid guid) => Guid.Equals(guid, Guid.Empty);
}