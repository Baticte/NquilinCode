namespace NquilinCode.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    public Task<Entities.User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}