namespace SiGameReborn.Common.Domain.Models;

public class SoftDeletableEntity : BaseEntity, ISoftDeletableEntity
{
    private DateTime? _deletedDate;

    public bool Deleted => DeletedDate != null;

    public DateTime? DeletedDate
    {
        get => _deletedDate;
        init => _deletedDate = value;
    }

    public void MarkAsDeleted() => _deletedDate = DateTime.Now;
}