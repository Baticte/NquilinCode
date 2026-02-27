using NquilinCode.Domain.Entities;

namespace NquilinCode.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User(CancellationToken cancellationToken);
}