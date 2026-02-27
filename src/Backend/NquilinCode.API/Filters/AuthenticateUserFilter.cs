using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using NquilinCode.Communication.Responses;
using NquilinCode.Domain.Repositories.User;
using NquilinCode.Domain.Security.Tokens;
using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;

namespace NquilinCode.API.Filters;

public class AuthenticateUserFilter : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator;
    private readonly IUserReadOnlyRepository _repository;

    public AuthenticateUserFilter(IAccessTokenValidator accessTokenValidator, IUserReadOnlyRepository repository)
    {
        _accessTokenValidator = accessTokenValidator;
        _repository = repository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);

            var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

            var exists = await _repository.ExistActiveUserWithIdentifier(userIdentifier, CancellationToken.None);
            if (!exists)
            {
                context.Result =
                    new UnauthorizedObjectResult(
                        new ResponseErrorJson(ValidationMessages.USER_WITHOUT_PERMISSION_TO_ACCESS_RESOURCE));
            }
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired")
            {
                TokenIsExpired = true
            });
        }
        catch (NquilinCodeException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            context.Result =
                new UnauthorizedObjectResult(
                    new ResponseErrorJson(ValidationMessages.USER_WITHOUT_PERMISSION_TO_ACCESS_RESOURCE));
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authorizationHeader  = context.HttpContext.Request.Headers.Authorization.ToString();

        return !authorizationHeader .StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? throw new NquilinCodeException(ValidationMessages.NO_TOKEN)
            : authorizationHeader ["Bearer ".Length..].Trim();
    }
}