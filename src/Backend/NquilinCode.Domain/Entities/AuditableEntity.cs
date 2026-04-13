namespace NquilinCode.Domain.Entities;

public abstract class AuditableEntity
{
    public Guid Id { get; protected set; }
    public bool Active { get; protected set; } = true;
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdateAt { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public Guid? DeletedBy { get; protected set; }
    
}
