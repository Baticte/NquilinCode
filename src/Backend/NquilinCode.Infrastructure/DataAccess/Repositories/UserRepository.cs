using Microsoft.EntityFrameworkCore;
using NquilinCode.Domain.Entities;
using NquilinCode.Domain.Repositories.User;

namespace NquilinCode.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly NquilinCodeDbContext _dbContext;

    public UserRepository(NquilinCodeDbContext dbContext) => _dbContext = dbContext;

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<bool> ExistActiveUserWithEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(x => x.Email.Equals(email) && x.Active, cancellationToken);
    }

    public async Task<User?> GetByEmailAndPasswordAsync(string email, string password,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Password.Equals(password),
                cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Active && u.Email == email, cancellationToken);
    }
}