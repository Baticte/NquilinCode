using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using NquilinCode.Domain.Security.Tokens;

namespace NquilinCode.Infrastructure.Security.Tokens.Access.Generator;

public class JwtAccessTokenGenerator : JwtAccessTokenHandler, IAccessTokenGenerator
{
    private readonly uint _accessTokenExpirationMinutes;
    private readonly string _signingKey;

    public JwtAccessTokenGenerator(uint accessTokenExpirationMinutes, string signingKey)
    {
        _accessTokenExpirationMinutes = accessTokenExpirationMinutes;
        _signingKey = signingKey;
    }

    public AccessTokenResult GenerateAccessToken(Guid userId)
    {
        var expirationDate = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, userId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            NotBefore = DateTime.UtcNow,
            IssuedAt = DateTime.UtcNow,
            Expires = expirationDate,
            
            Issuer = "NquilinCode.API",
            Audience = "NquilinCode.API",
            
            SigningCredentials = new SigningCredentials(
                SecurityKey(_signingKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return new AccessTokenResult
        {
            Token = tokenHandler.WriteToken(securityToken),
            ExpirationDate = expirationDate
        };
    }
}