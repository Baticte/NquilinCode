using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests.User;
using NquilinCode.Exceptions.Resources;
using Shouldly;
using WebApi.Test.Constants;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : NquilinCodeClassFixture
{
    private const string Method = ApiRoutes.Users.User;

    public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await DoPostAsync(Method, request, cancellationToken: TestContext.Current.CancellationToken);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync(TestContext.Current.CancellationToken);

        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: TestContext.Current.CancellationToken);
        responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNull().ShouldBe(request.Name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("tokens").GetProperty("accessTokenExpiration").GetString().ShouldNotBeNullOrWhiteSpace();
    }
    
    [Theory, ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var response = await DoPostAsync(Method, request, culture, cancellationToken: TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync(TestContext.Current.CancellationToken);
        
        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: TestContext.Current.CancellationToken);
        
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray().First();
        
        var expectedErrorMessage = ValidationMessages.ResourceManager.GetString("NAME_REQUIRED", new CultureInfo(culture));
        
        errors.GetString().ShouldNotBeNull().ShouldBe(expectedErrorMessage);
    }
}