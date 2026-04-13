using System.Net;
using CommonTestUtilities.Requests.User;
using CommonTestUtilities.Tokens;
using Shouldly;
using WebApi.Test.Constants;

namespace WebApi.Test.User.Update;

public class UpdateUserInvalidTokenTest : NquilinCodeClassFixture
{
    private const string Method = ApiRoutes.Users.User;

    public UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Error_Invalid_Token()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        
        var response = await DoPutAsync(Method, request, "invalid_Token", cancellationToken: TestContext.Current.CancellationToken);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        
        var response = await DoPutAsync(Method, request, string.Empty, cancellationToken: TestContext.Current.CancellationToken);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        
        var token = JwtAccessTokenGeneratorBuilder.Build().GenerateAccessToken(Guid.NewGuid());

        var response = await DoPutAsync(Method, request, token.Token, cancellationToken: TestContext.Current.CancellationToken);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}