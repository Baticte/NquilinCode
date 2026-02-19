using FluentValidation;
using NquilinCode.Application.UseCases.User.Register;
using NquilinCode.Communication.Requests;

namespace CommonTestUtilities.Validators;

public class ValidatorRegisterUserBuilder
{
    public static IValidator<RequestRegisterUserJson> Build()
    {
        return new RegisterUserValidator();
    }
}