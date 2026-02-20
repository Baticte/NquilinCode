using FluentValidation;
using NquilinCode.Application.UseCases.User.Login.DoLogin;
using NquilinCode.Communication.Requests;

namespace CommonTestUtilities.Validators;

public class ValidatorLoginBuilder
{
    public static IValidator<RequestLoginJson> Build()
    {
        return new LoginValidator();
    }
}