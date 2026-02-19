using FluentValidation;
using NquilinCode.Communication.Requests;
using NquilinCode.Exceptions.Resources;

namespace NquilinCode.Application.UseCases.User.Login.DoLogin;

public class LoginValidator : AbstractValidator<RequestLoginJson>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().NotEmpty()
            .WithMessage(ValidationMessages.EMAIL_REQUIRED);
        RuleFor(x => x.Password)
            .NotNull().NotEmpty()
            .WithMessage(ValidationMessages.PASSWORD_REQUIRED);
        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(ValidationMessages.INVALID_EMAIL_FORMAT);
        });
    }
}