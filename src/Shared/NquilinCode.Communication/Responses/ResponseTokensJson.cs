namespace NquilinCode.Communication.Responses;

public class ResponseTokensJson
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime AccessTokenExpiration { get; init; }
}