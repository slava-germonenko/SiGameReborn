using SiGameReborn.Tokens.Core.Dtos;
using SiGameReborn.Tokens.Core.Models;

namespace SiGameReborn.Tokens.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<RefreshToken> ApplyRefreshTokensFilter(
        this IQueryable<RefreshToken> refreshTokensQuery,
        RefreshTokensFilter filter
    )
    {
        var baseQuery = refreshTokensQuery;
        if (filter.UserId is not null)
        {
            baseQuery = baseQuery.Where(token => token.UserId == filter.UserId);
        }

        if (!string.IsNullOrEmpty(filter.IpAddress))
        {
            baseQuery = baseQuery.Where(token => token.RequestedByIpAddress == filter.IpAddress);
        }

        if (!string.IsNullOrEmpty(filter.DeviceName))
        {
            baseQuery = baseQuery.Where(token => token.RequestedByDevice == filter.DeviceName);
        }

        return baseQuery;
    }
}