namespace NquilinCode.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmailAsync(string email, CancellationToken cancellationToken);
    Task<Entities.User?> GetByEmailAndPasswordAsync(string email, string password, CancellationToken cancellationToken);
    Task<Entities.User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}