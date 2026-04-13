using Microsoft.EntityFrameworkCore;
using NquilinCode.Domain.Entities;
using NquilinCode.Domain.ValueObjects;

namespace NquilinCode.Infrastructure.DataAccess;

public class NquilinCodeDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NquilinCodeDbContext).Assembly);

        modelBuilder.Entity<User>(user =>
        {
            user.Property(u => u.Email)
                .HasConversion(
                    email => email.Value,
                    value => new Email(value)
                )
                .HasColumnName("Email");

            user.Property(u => u.Password)
                .HasConversion(
                    password => password.Value,
                    value => new Password(value)
                )
                .HasColumnName("Password");
        });
    }
}