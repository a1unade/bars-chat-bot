using FluentValidation;
using NotifyHub.Application.Requests.User;

namespace NotifyHub.Application.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Email должен быть валидным адресом.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name обязателен.")
            .MaximumLength(100).WithMessage("Name не должен превышать 100 символов.");

        RuleFor(x => x.TelegramUserId)
            .GreaterThan(0).WithMessage("TelegramUserId должен быть положительным числом.");
    }
}