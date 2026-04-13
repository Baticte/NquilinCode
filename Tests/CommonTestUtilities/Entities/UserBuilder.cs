using Bogus;
using CommonTestUtilities.Cryptography;
using NquilinCode.Domain.Entities;
using NquilinCode.Domain.ValueObjects;

namespace CommonTestUtilities.Entities;

public static class UserBuilder
{
    public static (User user, string password) Build()
    {
        var faker = new Faker();
        var passwordEncrypter = PasswordHasherBuilder.Build();

        var password = faker.Internet.Password();
        var passwordHash = passwordEncrypter.HashPassword(new Password(password));

        var user = User.Create(
            faker.Person.FullName,
            new Email(faker.Person.Email),
            new Password(passwordHash)
        );
        
        return (user, password);
    }
}