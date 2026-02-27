using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using NquilinCode.Domain.Security.Tokens;

namespace NquilinCode.Infrastructure.Security.Tokens.Access.Validator;

public class JwtAccessTokenValidator : JwtAccessTokenHandler, IAccessTokenValidator
{
    private readonly string _signingKey;

    public JwtAccessTokenValidator(string signingKey) => _signingKey = signingKey;

    public Guid ValidateAndGetUserIdentifier(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidAudience = "NquilinCode.API",
            ValidateIssuer = true,
            ValidIssuer = "NquilinCode.API",
            IssuerSigningKey = SecurityKey(_signingKey),
            ClockSkew = new TimeSpan(0)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

        var userIdentifier = principal.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

        return string.IsNullOrWhiteSpace(userIdentifier)
            ? throw new SecurityTokenException("Invalid token: missing user identifier.")
            : Guid.Parse(userIdentifier);
    }
}