using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests.User;
using NquilinCode.Communication.Requests;
using NquilinCode.Exceptions.Resources;
using Shouldly;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Login.DoLogin;

public class LoginTest : NquilinCodeClassFixture
{
    private readonly string _method = "/login";
    private readonly string _email;
    private readonly string _password;
    private readonly string _name;
    
    public LoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _name = factory.GetName();
    }
    
    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };
        
        var response = await DoPostAsync(_method, request, cancellationToken: TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync(TestContext.Current.CancellationToken);

        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: TestContext.Current.CancellationToken);
        responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNull().ShouldBe(_name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("tokens").GetProperty("accessTokenExpiration").GetString().ShouldNotBeNullOrWhiteSpace();
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();
        
        var response = await DoPostAsync(_method, request, culture, cancellationToken: TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync(TestContext.Current.CancellationToken);
        
        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: TestContext.Current.CancellationToken);
        
        var errorMessages = responseData.RootElement.GetProperty("errors").EnumerateArray().First();
        
        var expectedErrorMessage = ValidationMessages.ResourceManager.GetString("INVALID_EMAIL_OR_PASSWORD", new CultureInfo(culture));
        
        errorMessages.GetString().ShouldNotBeNull().ShouldBe(expectedErrorMessage);
    }
}