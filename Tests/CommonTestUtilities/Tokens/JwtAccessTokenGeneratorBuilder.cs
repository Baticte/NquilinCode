using NquilinCode.Domain.Security.Tokens;
using NquilinCode.Infrastructure.Security.Tokens.Access.Generator;

namespace CommonTestUtilities.Tokens;

public static class JwtAccessTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtAccessTokenGenerator(accessTokenExpirationMinutes: 5,
        signingKey: "123456789123456789123456789123456");
}