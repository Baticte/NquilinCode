using CommonTestUtilities.Entities;
using CommonTestUtilities.Services.LoggedUser;
using NquilinCode.Application.UseCases.User.Profile;
using Shouldly;

namespace UseCase.Test.User.Profile;

public class GetUserProfileTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(CancellationToken.None);
        
        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Email.ShouldBe(user.Email);
    }

    private static GetUserProfileUseCase CreateUseCase(NquilinCode.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetUserProfileUseCase(loggedUser);
    }
}