using Moq;
using NquilinCode.Domain.Entities;
using NquilinCode.Domain.Services.LoggedUser;

namespace CommonTestUtilities.Services.LoggedUser;

public static class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(x => x.User(CancellationToken.None)).ReturnsAsync(user);

        return mock.Object;
    }
}