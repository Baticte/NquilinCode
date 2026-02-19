using NquilinCode.Exceptions.Resources;

namespace NquilinCode.Exceptions.BaseException;

public class InvalidLoginException : NquilinCodeException
{
    public InvalidLoginException() : base(ValidationMessages.INVALID_EMAIL_OR_PASSWORD)
    {
    }
}