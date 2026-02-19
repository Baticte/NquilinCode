namespace NquilinCode.Exceptions.BaseException;

public class RegisterUserValidationException : NquilinCodeException
{
    public IReadOnlyList<string> ErrorMessages { get; }
    public RegisterUserValidationException(IEnumerable<string> errorMessages) : base(string.Empty) => ErrorMessages = errorMessages.ToList();
    
    public RegisterUserValidationException(string errorMessage)
        : this(new List<string> { errorMessage })
    {
    }
}