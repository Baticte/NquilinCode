using Bogus;
using CommonTestUtilities.Cryptography;
using NquilinCode.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        var passwordEncripter = PasswordHasherBuilder.Build();
        
        var password = new Faker().Internet.Password();
        
        var user = new Faker<User>()
            .RuleFor(u => u.Id, (f, u) => Guid.NewGuid())
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => passwordEncripter.HashPassword(password))
            .Generate();
        
        return (user, password);
    }
}