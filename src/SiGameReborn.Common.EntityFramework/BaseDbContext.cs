using Microsoft.EntityFrameworkCore;

using SiGameReborn.Common.Domain.Models;

namespace SiGameReborn.Common.EntityFramework;

public abstract class BaseDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions options) : base(options) { }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetCreatedDateOnCratedEntities();
        SetUpdatedDateOnUpdatedEntries();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new())
    {
        SetCreatedDateOnCratedEntities();
        SetUpdatedDateOnUpdatedEntries();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void SetCreatedDateOnCratedEntities()
    {
        var updatedEntries = ChangeTracker.Entries<IBaseEntity>().Where(entry => entry.State == EntityState.Modified);
        foreach (var updatedEntry in updatedEntries)
        {
            updatedEntry.Entity.UpdatedDate = null;
            updatedEntry.Entity.UpdatedDate = DateTime.UtcNow;
        }
    }

    private void SetUpdatedDateOnUpdatedEntries()
    {
        var updatedEntries = ChangeTracker.Entries<IBaseEntity>().Where(entry => entry.State == EntityState.Modified);
        foreach (var updatedEntry in updatedEntries)
        {
            updatedEntry.Property(entity => entity.CreatedDate).IsModified = false;
            updatedEntry.Entity.UpdatedDate = DateTime.UtcNow;
        }
    }
}