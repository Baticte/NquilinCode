using Moq;
using NquilinCode.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories.User;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder() => _repository = new Mock<IUserReadOnlyRepository>();

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }

    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(r => r.ExistActiveUserWithEmailAsync(email, CancellationToken.None))
            .ReturnsAsync(true);
    }
}