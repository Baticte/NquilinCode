using NquilinCode.Application.Abstractions.Services.Cryptography;
using NquilinCode.Domain.ValueObjects;

namespace NquilinCode.Application.Services.Cryptography;

public class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Gera um hash seguro. O BCrypt gera e inclui o Salt automaticamente na string final.
    /// </summary>
    /// <param name="password">Password em texto limpo.</param>
    /// <returns>Hash formatado (ex: $2a$11$SaltHash...)</returns>
    public string HashPassword(Password password)
    {
        // O workFactor (custo) padrão é 11. 
        // Podes aumentar para 12 ou 13 se quiseres ainda mais segurança, 
        // mas 11 é o equilíbrio perfeito entre segurança e performance.
        return BCrypt.Net.BCrypt.HashPassword(password.Value, workFactor: 11);
    }

    /// <summary>
    /// Verifica se a password coincide com o hash guardado na base de dados.
    /// </summary>
    public bool VerifyPassword(Password password, Password hashedPassword)
    {
        // O BCrypt extrai o Salt e o custo do próprio hashedPassword para validar.
        return BCrypt.Net.BCrypt.Verify(password.Value, hashedPassword.Value);
    }
}