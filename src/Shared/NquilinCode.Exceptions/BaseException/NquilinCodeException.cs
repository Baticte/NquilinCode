namespace NquilinCode.Exceptions.BaseException;

public abstract class NquilinCodeException : Exception
{
    protected NquilinCodeException(string message) : base(message) { }
}