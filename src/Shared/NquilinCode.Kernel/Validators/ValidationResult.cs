namespace NquilinCode.Kernel.Validators;

public class ValidationResult
{
    public bool IsSuccess => Errors.Count == 0;
    public List<string> Errors { get; }

    private ValidationResult(List<string> errors)
    {
        Errors = errors;
    }

    public static ValidationResult Success() => new ValidationResult(new List<string>());
    public static ValidationResult Failure(params string[] errors) => new ValidationResult(errors.ToList());
    public static ValidationResult Failure(IEnumerable<string> errors) => new ValidationResult(errors.ToList());

    public void Merge(ValidationResult other)
    {
        Errors.AddRange(other.Errors);
    }
}