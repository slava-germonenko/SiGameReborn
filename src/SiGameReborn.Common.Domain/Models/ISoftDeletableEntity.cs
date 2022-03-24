namespace SiGameReborn.Common.Domain.Models;

public interface ISoftDeletableEntity
{
    public DateTime? DeletedDate { get; }

    void MarkAsDeleted();
}