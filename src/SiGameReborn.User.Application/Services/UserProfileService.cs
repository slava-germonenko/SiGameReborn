using Microsoft.EntityFrameworkCore;

using SiGameReborn.Common.Domain.Exceptions;
using SiGameReborn.Common.EntityFramework.Extensions;
using SiGameReborn.User.Core;
using SiGameReborn.User.Core.Models;
using SiGameReborn.User.Core.Services;

namespace SiGameReborn.User.Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly UserContext _userContext;

    public UserProfileService(UserContext userContext)
    {
        _userContext = userContext;
    }

    public async Task<UserProfile> GetUserProfileAsync(Guid userProfileId)
    {
        var userProfile = await _userContext.UserProfiles.FirstOrNotFoundExceptionAsync(
            user => user.Id == userProfileId,
            $"Профиль пользователя c идентификатором \"{userProfileId}\" не найден."
        );

        EnsureUserIsNotDeleted(userProfile);
        return userProfile;
    }

    public async Task<UserProfile> GetUserProfileByEmailAddressAsync(string emailAddress)
    {
        var userProfile = await _userContext.UserProfiles.FirstOrNotFoundExceptionAsync(
            user => user.EmailAddress == emailAddress,
            $"Профиль пользователя с адресом электронной почты \"{emailAddress}\" не найден."
        );

        EnsureUserIsNotDeleted(userProfile);
        return userProfile;
    }

    public async Task<UserProfile> GetUserProfileByUsernameAsync(string userName)
    {
        var userProfile = await _userContext.UserProfiles.FirstOrNotFoundExceptionAsync(
            user => user.Username == userName,
            $"Профиль пользователя с именем пользователя \"{userName}\" не найден."
        );

        EnsureUserIsNotDeleted(userProfile);
        return userProfile;
    }

    public async Task<UserProfile> SaveUserProfileAsync(UserProfile sourceUserProfile)
    {
        var profile = await _userContext.UserProfiles.FindAsync(sourceUserProfile.Id)
            ?? new UserProfile{Id = sourceUserProfile.Id};
        if (profile.Deleted)
        {
            throw new CoreLogicException("Профиль пользователя не активен.");
        }

        var emailAddressIsInUse = await _userContext.UserProfiles.AnyAsync(
            user => user.EmailAddress == sourceUserProfile.EmailAddress && user.Id != sourceUserProfile.Id
        );
        if (emailAddressIsInUse)
        {
            throw new DuplicateException($"Адрес электронной почты \"{sourceUserProfile.EmailAddress}\" уже занят.");
        }

        var usernameIsInUse = await _userContext.UserProfiles.AnyAsync(
            user => user.Username == sourceUserProfile.Username && user.Id != sourceUserProfile.Id
        );
        if (usernameIsInUse)
        {
            throw new DuplicateException($"Имя пользователя \"{sourceUserProfile.Username}\" уже занято.");
        }

        profile.CopyDetails(sourceUserProfile);
        _userContext.UserProfiles.Update(profile);
        await _userContext.SaveChangesAsync();
        return profile;
    }

    public async Task SoftDeletedUserAsync(Guid userId)
    {
        var user = await _userContext.UserProfiles.FindOrNotFoundException(
            userId,
            $"Пользователь с идентификатором {userId} не найден"
        );

        user.MarkAsDeleted();
        _userContext.UserProfiles.Update(user);
        await _userContext.SaveChangesAsync();
    }

    private static void EnsureUserIsNotDeleted(UserProfile userProfile)
    {
        if (userProfile.Deleted)
        {
            throw new NotFoundException($"Пользователь с идентификатором {userProfile.Id} был удалён.");
        }
    }
}