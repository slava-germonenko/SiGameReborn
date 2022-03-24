using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using SiGameReborn.Common.Domain.Exceptions;
using SiGameReborn.Common.Domain.Models.Paging;

namespace SiGameReborn.Common.EntityFramework.Extensions;

public static class QueryableExtensions
{
    public static async Task<TEntity> FirstOrNotFoundExceptionAsync<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>> predicateExpression,
        string? exceptionMessage = null
    )
    {
        var entity = await query.FirstOrDefaultAsync(predicateExpression);
        if (entity is null)
        {
            throw new NotFoundException(exceptionMessage);
        }

        return entity;
    }

    public static async Task<PagedResult<TEntity>> ToPagedResult<TEntity>(
        this IQueryable<TEntity> query,
        PageDescriptor pageDescriptor
    )
    {
        var items = await query.Skip(pageDescriptor.Offset).Take(pageDescriptor.Count).ToListAsync();
        var count = await query.CountAsync();
        return new PagedResult<TEntity>
        {
            Items = items,
            Total = count,
            Page = pageDescriptor,
        };
    }
}