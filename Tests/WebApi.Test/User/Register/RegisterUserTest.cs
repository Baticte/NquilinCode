using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests.User;
using NquilinCode.Exceptions.Resources;
using Shouldly;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public RegisterUserTest(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync("/user", request, cancellationToken: TestContext.Current.CancellationToken);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync(TestContext.Current.CancellationToken);

        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: TestContext.Current.CancellationToken);
        responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNull().ShouldBe(request.Name);
    }
    
    [Theory, ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        if(_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
        
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
        
        var response = await _httpClient.PostAsJsonAsync("/user", request, cancellationToken: TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync(TestContext.Current.CancellationToken);
        
        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: TestContext.Current.CancellationToken);
        
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray().First();
        
        var expectedErrorMessage = ValidationMessages.ResourceManager.GetString("NAME_REQUIRED", new CultureInfo(culture));
        
        errors.GetString().ShouldNotBeNull().ShouldBe(expectedErrorMessage);
    }
}