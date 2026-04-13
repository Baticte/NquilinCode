using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests.User;
using CommonTestUtilities.Tokens;
using NquilinCode.Exceptions.Resources;
using Shouldly;
using WebApi.Test.Constants;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;

public class UpdateUserTest : NquilinCodeClassFixture
{
    private const string Method = ApiRoutes.Users.User;

    private readonly Guid _userIdentifier;

    public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtAccessTokenGeneratorBuilder.Build().GenerateAccessToken(_userIdentifier);

        var response = await DoPutAsync(Method, request, token.Token,
            cancellationToken: TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var token = JwtAccessTokenGeneratorBuilder.Build().GenerateAccessToken(_userIdentifier);

        var response = await DoPutAsync(Method, request, token.Token, culture,
            cancellationToken: TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody =
            await response.Content.ReadAsStreamAsync(cancellationToken: TestContext.Current.CancellationToken);

        var responseData =
            await JsonDocument.ParseAsync(responseBody, cancellationToken: TestContext.Current.CancellationToken);

        var errorMessages = responseData.RootElement.GetProperty("errors").EnumerateArray().First();

        var expectedErrorMessage =
            ValidationMessages.ResourceManager.GetString("NAME_REQUIRED", new CultureInfo(culture));

        errorMessages.GetString().ShouldNotBeNull().ShouldBe(expectedErrorMessage);
    }
}