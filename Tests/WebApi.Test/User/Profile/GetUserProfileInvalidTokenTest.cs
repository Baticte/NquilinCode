using System.Net;
using CommonTestUtilities.Tokens;
using NquilinCode.Infrastructure.Security.Tokens.Access.Generator;
using Shouldly;

namespace WebApi.Test.User.Profile;

public class GetUserProfileInvalidTokenTest : NquilinCodeClassFixture
{
    private readonly string _method = "/user";
    
    public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Error_Invalid_Token()
    {
        var response = await DoGetAsync(_method, "invalid_Token", cancellationToken: TestContext.Current.CancellationToken);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGetAsync(_method, string.Empty, cancellationToken: TestContext.Current.CancellationToken);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtAccessTokenGeneratorBuilder.Build().GenerateAccessToken(Guid.NewGuid());

        var response = await DoGetAsync(_method, token.Token, cancellationToken: TestContext.Current.CancellationToken);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}