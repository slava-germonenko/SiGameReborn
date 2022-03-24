using SiGameReborn.Common.Domain.Models.Paging;
using SiGameReborn.User.Core.Models;
using SiGameReborn.User.Core.Models.Dtos;

namespace SiGameReborn.User.Core.Services;

public interface IUsersListService
{
    public Task<PagedResult<UserProfile>> GetUsersListAsync(PageDescriptor page, UserProfilesFilter? filter = null);
}