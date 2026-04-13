using Moq;
using NquilinCode.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories.User;

public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _mock = new();

    public IUserUpdateOnlyRepository Build() => _mock.Object;

    public UserUpdateOnlyRepositoryBuilder GetById(NquilinCode.Domain.Entities.User user)
    {
        _mock.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        return this;
    }
}