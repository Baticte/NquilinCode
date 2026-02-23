using System.Net.Http.Json;

namespace WebApi.Test.User;

public class NquilinCodeClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    protected NquilinCodeClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPostAsync(string method, object request, string culture = "en", CancellationToken cancellationToken = default)
    {
        ChangeRequiredCulture(culture);
        return await _httpClient.PostAsJsonAsync(method, request, cancellationToken: TestContext.Current.CancellationToken);
    }

    private void ChangeRequiredCulture(string culture)
    {
        if(_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
        
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }
}