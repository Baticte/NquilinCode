using Moq;
using NquilinCode.Application.Abstractions.Services.Cryptography;
using NquilinCode.Application.Services.Cryptography;

namespace CommonTestUtilities.Cryptography;

public static class PasswordHasherBuilder
{
    public static IPasswordHasher Build() => new PasswordHasher();
}