using NquilinCode.Domain.ValueObjects;

namespace NquilinCode.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmailAsync(Email email, CancellationToken cancellationToken);
    Task<Entities.User?> GetByEmailAndPasswordAsync(Email email, Password password, CancellationToken cancellationToken);
    Task<Entities.User?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
    Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier, CancellationToken cancellationToken);
    Task<Entities.User> GetByUserWithIdentifier(Guid userIdentifier, CancellationToken cancellationToken);
}