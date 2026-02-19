namespace NquilinCode.Application.Abstractions.Services.Cryptography;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}