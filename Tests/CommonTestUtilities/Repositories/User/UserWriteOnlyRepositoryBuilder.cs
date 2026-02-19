using Moq;
using NquilinCode.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories.User;

public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}