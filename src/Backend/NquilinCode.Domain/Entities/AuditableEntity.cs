namespace NquilinCode.Domain.Entities;

public abstract class AuditableEntity
{
    public Guid Id { get; protected set; }

    public bool Active { get; private set; } = true;

    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedBy { get; private set; }

    public void Delete(Guid deletedBy)
    {
        if (!Active)
            return;

        Active = false;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}
