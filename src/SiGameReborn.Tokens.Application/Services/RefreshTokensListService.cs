using Microsoft.EntityFrameworkCore;

using SiGameReborn.Common.Domain.Models.Paging;
using SiGameReborn.Common.EntityFramework.Extensions;
using SiGameReborn.Tokens.Application.Extensions;
using SiGameReborn.Tokens.Core;
using SiGameReborn.Tokens.Core.Dtos;
using SiGameReborn.Tokens.Core.Models;
using SiGameReborn.Tokens.Core.Services;

namespace SiGameReborn.Tokens.Application.Services;

public class RefreshTokensListService : IRefreshTokensListService
{
    private readonly TokensContext _context;

    public RefreshTokensListService(TokensContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<RefreshToken>> GetRefreshTokensListAsync(
        PageDescriptor page,
        RefreshTokensFilter filter
    )
    {
        return await _context.RefreshTokens.AsNoTracking()
            .ApplyRefreshTokensFilter(filter)
            .ToPagedResult(page);
    }
}