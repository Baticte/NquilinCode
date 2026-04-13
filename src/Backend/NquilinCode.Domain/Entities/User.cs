using NquilinCode.Domain.ValueObjects;
using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;

namespace NquilinCode.Domain.Entities;

public class User : AuditableEntity
{
    public string Name { get; private set; } =  string.Empty;
    public Email Email { get; private set; } =  null!;
    public Password Password { get; private set; } =  null!;

    public static User Create(string name, Email email, Password passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new NquilinCodeException(ValidationMessages.NAME_REQUIRED);
            
        return new User
        {
            Name = name,
            Email = email,
            Password = passwordHash
        };
    }
    
    public void Update(string name, Email email)
    {
        Name = name;
        Email = email;
        UpdateAt = DateTime.UtcNow;
    }
    
    public void Delete(Guid deletedBy)
    {
        if (!Active)
            return;

        Active = false;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}