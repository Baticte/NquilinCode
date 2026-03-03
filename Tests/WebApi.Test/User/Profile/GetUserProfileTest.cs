using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using Shouldly;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest : NquilinCodeClassFixture
{
    private readonly string _method = "/user";

    private readonly Guid _userIdentifier;
    private readonly string _name;
    private readonly string _email;

    public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetId();
        _name = factory.GetName();
        _email = factory.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtAccessTokenGeneratorBuilder.Build().GenerateAccessToken(_userIdentifier);

        var response = await DoGetAsync(_method, token.Token, cancellationToken: TestContext.Current.CancellationToken);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync(TestContext.Current.CancellationToken);

        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: TestContext.Current.CancellationToken);
        
        responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        responseData.RootElement.GetProperty("email").GetString().ShouldNotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("email").GetString().ShouldBe(_email);
    }

}