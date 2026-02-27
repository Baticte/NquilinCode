using NquilinCode.Domain.Security.Tokens;
using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;

namespace NquilinCode.API.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextTokenValue(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    public string Value()
    {
        var authentication = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
        
        return !authentication.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? throw new NquilinCodeException(ValidationMessages.NO_TOKEN)
            : authentication["Bearer ".Length..].Trim();
    }
}