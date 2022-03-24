using Microsoft.EntityFrameworkCore;

using SiGameReborn.Common.Domain.Models.Paging;
using SiGameReborn.Common.EntityFramework.Extensions;
using SiGameReborn.User.Core;
using SiGameReborn.User.Core.Models;
using SiGameReborn.User.Core.Models.Dtos;
using SiGameReborn.User.Core.Services;

namespace SiGameReborn.User.Application.Services;

public class UsersListService : IUsersListService
{
    private readonly UserContext _context;

    public UsersListService(UserContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<UserProfile>> GetUsersListAsync(PageDescriptor page, UserProfilesFilter? filter = null)
    {
        var usersQuery = _context.UserProfiles.AsNoTracking();
        if (!string.IsNullOrEmpty(filter?.Search))
        {
            usersQuery = usersQuery.Where(
                user => user.Username.Contains(filter.Search) || user.EmailAddress.Contains(filter.Search)
            );
        }

        return await usersQuery.ToPagedResult(page);
    }
}