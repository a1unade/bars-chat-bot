using FluentValidation;
using NotifyHub.NotificationService.Application.Common.Requests;

namespace NotifyHub.NotificationService.Application.Validators;

public class IdRequestValidator : AbstractValidator<IdRequest>
{
    public IdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID не должен быть пустым.");
    }
}
