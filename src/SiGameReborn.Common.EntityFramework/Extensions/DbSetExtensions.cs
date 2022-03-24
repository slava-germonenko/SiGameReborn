using Microsoft.EntityFrameworkCore;

using SiGameReborn.Common.Domain.Exceptions;

namespace SiGameReborn.Common.EntityFramework.Extensions;

public static class DbSetExtensions
{
    public static async Task<TEntity> FindOrNotFoundException<TEntity>(
        this DbSet<TEntity> dbSet,
        object pk,
        string? exceptionMessage = null
    ) where TEntity : class
    {
        var foundEntity = await dbSet.FindAsync(pk);
        if (foundEntity is null)
        {
            throw new NotFoundException(exceptionMessage);
        }

        return foundEntity;
    }
}