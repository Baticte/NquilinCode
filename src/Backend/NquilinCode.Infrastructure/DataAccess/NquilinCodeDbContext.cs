using Microsoft.EntityFrameworkCore;
using NquilinCode.Domain.Entities;

namespace NquilinCode.Infrastructure.DataAccess;

public class NquilinCodeDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NquilinCodeDbContext).Assembly);
    }
}