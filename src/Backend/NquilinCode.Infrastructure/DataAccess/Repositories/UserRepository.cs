using Microsoft.EntityFrameworkCore;
using NquilinCode.Domain.Entities;
using NquilinCode.Domain.Repositories.User;
using NquilinCode.Domain.ValueObjects;

namespace NquilinCode.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly NquilinCodeDbContext _dbContext;

    public UserRepository(NquilinCodeDbContext dbContext) => _dbContext = dbContext;

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<bool> ExistActiveUserWithEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(x => x.Email == email && x.Active, cancellationToken);
    }

    public async Task<User?> GetByEmailAndPasswordAsync(Email email, Password password,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active && user.Email == email && user.Password == password,
                cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Active && u.Email == email, cancellationToken);
    }

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AnyAsync(user => user.Active && user.Id == userIdentifier, cancellationToken);
    }

    public async Task<User> GetByUserWithIdentifier(Guid userIdentifier, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.Id == userIdentifier, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Active && user.Id == id, cancellationToken);
    }
}