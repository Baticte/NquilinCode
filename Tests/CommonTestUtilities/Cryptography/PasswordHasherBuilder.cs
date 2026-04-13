using NquilinCode.Domain.Security.Cryptography;
using NquilinCode.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public static class PasswordHasherBuilder
{
    public static IPasswordHasher Build() => new PasswordHasher();
}