using FluentValidation;
using NquilinCode.Communication.Requests;
using NquilinCode.Exceptions.Resources;

namespace NquilinCode.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ValidationMessages.NAME_REQUIRED);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationMessages.EMAIL_REQUIRED);

        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage(ValidationMessages.INVALID_EMAIL_FORMAT);
        });
    }
}