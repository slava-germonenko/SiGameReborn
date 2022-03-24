using SiGameReborn.Common.Domain.Exceptions;
using SiGameReborn.User.Core.Models;
using SiGameReborn.User.Core.Services;

namespace SiGameReborn.User.Api.Extensions;

public static class UserProfileServiceExtensions
{
    public static async Task<UserProfile> GetUserProfileByEmailOrUsername(
        this IUserProfileService userProfileService,
        string usernameOrEmail
    )
    {
        UserProfile? userProfile = await userProfileService.TryGetUserProfileByEmailAddressAsync(usernameOrEmail)
            ?? await userProfileService.TryGetUserProfileByUsernameAddressAsync(usernameOrEmail);

        if (userProfile is null)
        {
            throw new NotFoundException("Пользователь не найден.");
        }

        return userProfile;
    }

    private static async Task<UserProfile?> TryGetUserProfileByEmailAddressAsync(
        this IUserProfileService userProfileService,
        string emailAddress
    )
    {
        try
        {
            return await userProfileService.GetUserProfileByEmailAddressAsync(emailAddress);
        }
        catch(NotFoundException)
        {
            return null;
        }
    }

    private static async Task<UserProfile?> TryGetUserProfileByUsernameAddressAsync(
        this IUserProfileService userProfileService,
        string username
    )
    {
        try
        {
            return await userProfileService.GetUserProfileByUsernameAsync(username);
        }
        catch(NotFoundException)
        {
            return null;
        }
    }
}