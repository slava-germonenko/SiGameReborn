using SiGameReborn.Common.Domain.Models.Paging;
using SiGameReborn.Tokens.Core.Dtos;
using SiGameReborn.Tokens.Core.Models;

namespace SiGameReborn.Tokens.Core.Services;

public interface IRefreshTokensListService
{
    public Task<PagedResult<RefreshToken>> GetRefreshTokensListAsync(
        PageDescriptor page,
        RefreshTokensFilter filter
    );
}