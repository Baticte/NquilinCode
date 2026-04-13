using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;

namespace NquilinCode.Domain.ValueObjects;

public class Password
{
    public string Value { get; }

    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new RegisterUserValidationException(ValidationMessages.PASSWORD_REQUIRED);

        if (value.Length < 6)
            throw new RegisterUserValidationException(string.Format(ValidationMessages.PASSWORD_MIN_LENGTH, 6));

        Value = value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Password password && password.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}