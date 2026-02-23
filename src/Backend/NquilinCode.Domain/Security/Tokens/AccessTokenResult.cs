namespace NquilinCode.Domain.Security.Tokens;

public class AccessTokenResult
{
    public string Token { get; init; } = string.Empty;
    public DateTime ExpirationDate { get; init; }
}