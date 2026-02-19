using Moq;
using NquilinCode.Application.Abstractions.Services.Cryptography;
using NquilinCode.Application.Services.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordHasherBuilder
{
    public static IPasswordHasher Build()
    {
        var mock = new Mock<IPasswordHasher>();

        return mock.Object;
    }
}