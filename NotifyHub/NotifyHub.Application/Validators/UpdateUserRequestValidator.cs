using FluentValidation;
using NotifyHub.Application.Requests.User;

namespace NotifyHub.Application.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id пользователя обязателен.");

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email!)
                .EmailAddress().WithMessage("Email должен быть валидным.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name!)
                .NotEmpty().WithMessage("Name не может быть пустым.")
                .MaximumLength(100).WithMessage("Name не должен превышать 100 символов.");
        });
    }
}