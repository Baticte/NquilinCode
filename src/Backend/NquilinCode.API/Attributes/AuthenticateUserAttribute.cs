using Microsoft.AspNetCore.Mvc;
using NquilinCode.API.Filters;

namespace NquilinCode.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class AuthenticateUserAttribute : TypeFilterAttribute
{
    public AuthenticateUserAttribute() : base(typeof(AuthenticateUserFilter))
    {
    }
}