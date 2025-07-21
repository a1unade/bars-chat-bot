using FluentValidation;
using NotifyHub.Application.Requests.User;

namespace NotifyHub.Application.Validators;

public class UpdateUserValidation : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidation()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID пользователя должен быть положительным числом!");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Указан некорректный email адрес!")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Имя не может быть пустым!")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Имя не может состоять только из пробелов!")
            .Length(1, 50)
            .WithMessage("Имя должно быть от 1 до 50 символов!");

        RuleFor(x => x.LastName)
            .Must(surname => !string.IsNullOrWhiteSpace(surname))
            .WithMessage("Фамилия не может состоять только из пробелов!")
            .Length(1, 50)
            .WithMessage("Фамилия должна быть от 1 до 50 символов!")
            .When(x => x.LastName != null);

        RuleFor(x => x.TelegramTag)
            .Must(tag => (tag.StartsWith("@") && tag.Length > 1))
            .WithMessage("Тег Telegram должен начинаться с @ и содержать имя пользователя.")
            .When(x => !string.IsNullOrWhiteSpace(x.TelegramTag));
    }
}
