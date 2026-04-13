using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;

namespace NquilinCode.Domain.ValueObjects;

public class Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
        {
            throw new RegisterUserValidationException(ValidationMessages.INVALID_EMAIL_FORMAT);
        }

        Value = value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Email other && Value == other.Value;
    }
    
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}