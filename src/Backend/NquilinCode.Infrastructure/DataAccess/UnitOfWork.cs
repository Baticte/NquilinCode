using NquilinCode.Domain.Repositories;

namespace NquilinCode.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly NquilinCodeDbContext _dbContext;

    public UnitOfWork(NquilinCodeDbContext dbContext) => _dbContext = dbContext;

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}