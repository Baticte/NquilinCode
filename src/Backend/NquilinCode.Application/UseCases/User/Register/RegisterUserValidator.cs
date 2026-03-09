using FluentValidation;
using NquilinCode.Communication.Requests;
using NquilinCode.Exceptions.Resources;

namespace NquilinCode.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().NotEmpty()
            .WithMessage(ValidationMessages.NAME_REQUIRED);
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
        When(user => !string.IsNullOrEmpty(user.Password), () =>
        {
            RuleFor(x => x.Password)
                .MinimumLength(6)
                .WithMessage(string.Format(ValidationMessages.PASSWORD_MIN_LENGTH, Constants.MinimumLengthPassword));
        });
    }
}