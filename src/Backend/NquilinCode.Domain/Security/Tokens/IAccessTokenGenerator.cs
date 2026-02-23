namespace NquilinCode.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    public AccessTokenResult GenerateAccessToken(Guid userId);
}