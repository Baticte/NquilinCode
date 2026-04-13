using NquilinCode.Domain.ValueObjects;

namespace NquilinCode.Application.Abstractions.Services.Cryptography;

public interface IPasswordHasher
{
    string HashPassword(Password password);
    bool VerifyPassword(Password password, Password hashedPassword);
}