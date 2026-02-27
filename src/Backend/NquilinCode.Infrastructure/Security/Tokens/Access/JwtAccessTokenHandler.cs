using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NquilinCode.Infrastructure.Security.Tokens.Access;

public abstract class JwtAccessTokenHandler
{
    protected static SymmetricSecurityKey SecurityKey(string signingKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signingKey);
        return new SymmetricSecurityKey(bytes);
    }
}