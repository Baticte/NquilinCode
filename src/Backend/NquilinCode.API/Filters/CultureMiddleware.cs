using System.Globalization;

namespace NquilinCode.API.Filters;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures);

        var requestCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        var cultureInfo = new CultureInfo("en");
        
        if (!string.IsNullOrEmpty(requestCulture) && supportedLanguages.Any(c => c.Name.Equals(requestCulture)))
        {
            cultureInfo = new CultureInfo(requestCulture);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}