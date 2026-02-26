using Microsoft.AspNetCore.Mvc;
using NquilinCode.API.Filters;

namespace NquilinCode.API.Attributes;

public class AuthenticateUserAttribute : TypeFilterAttribute
{
    public AuthenticateUserAttribute() : base(typeof(AuthenticateUserFilter))
    {
    }
}