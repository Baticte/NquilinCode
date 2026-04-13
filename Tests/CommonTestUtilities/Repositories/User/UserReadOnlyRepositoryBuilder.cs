using Moq;
using NquilinCode.Domain.Repositories.User;
using NquilinCode.Domain.ValueObjects;

namespace CommonTestUtilities.Repositories.User;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository = new();

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }

    public void ExistActiveUserWithEmail(Email email)
    {
        _repository.Setup(r => r.ExistActiveUserWithEmailAsync(
                It.Is<Email>(e => e.Value == email.Value),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }
    
    public void GetByEmailAsync(NquilinCode.Domain.Entities.User user)
    {
        _repository.Setup(r => r.GetByEmailAsync(It.Is<Email>(e => e.Value == user.Email.Value),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
    }
}