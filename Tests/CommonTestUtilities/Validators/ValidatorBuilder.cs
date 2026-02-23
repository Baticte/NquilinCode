using FluentValidation;

namespace CommonTestUtilities.Validators;

public static class ValidatorBuilder
{
    public static IValidator<T> Build<TValidator, T>()
        where TValidator : IValidator<T>, new()
    {
        return new TValidator();
    }
}