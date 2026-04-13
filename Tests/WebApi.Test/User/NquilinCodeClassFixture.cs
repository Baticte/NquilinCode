using System.Net.Http.Headers;
using System.Net.Http.Json;
using NquilinCode.Communication.Requests;

namespace WebApi.Test.User;

public class NquilinCodeClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    protected NquilinCodeClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPostAsync(string method, object request, string culture = "en",
        CancellationToken cancellationToken = default)
    {
        ChangeRequiredCulture(culture);
        return await _httpClient.PostAsJsonAsync(method, request,
            cancellationToken: TestContext.Current.CancellationToken);
    }

    protected async Task<HttpResponseMessage> DoGetAsync(string method, string token = "", string culture = "en",
        CancellationToken cancellationToken = default)
    {
        ChangeRequiredCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.GetAsync(method, cancellationToken: TestContext.Current.CancellationToken);
    }

    protected async Task<HttpResponseMessage> DoPutAsync(string method, RequestUpdateUserJson request, string token,
        string culture = "en", CancellationToken cancellationToken = default)
    {
        ChangeRequiredCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.PutAsJsonAsync(method, request, cancellationToken: TestContext.Current.CancellationToken);
    }

    private void ChangeRequiredCulture(string culture)
    {
        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }

    private void AuthorizeRequest(string token)
    {
        if (string.IsNullOrEmpty(token))
            return;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}