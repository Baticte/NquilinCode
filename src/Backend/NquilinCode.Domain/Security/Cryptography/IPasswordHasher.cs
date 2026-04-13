using NquilinCode.Domain.ValueObjects;

namespace NquilinCode.Domain.Security.Cryptography;

public interface IPasswordHasher
{
    string HashPassword(Password password);
    bool VerifyPassword(Password password, Password hashedPassword);
}