using SiGameReborn.User.Core.Models;

namespace SiGameReborn.User.Core.Services;

public interface IUserProfileService
{
    /// <summary>
    ///     Gets user by the given ID.
    /// </summary>
    ///     <param name="userProfileId">User ID to search by.</param>
    /// <exception cref="SiGameReborn.Common.Domain.Exceptions.NotFoundException">
    ///     Is thrown if there is no user profile with such an ID or if this user is soft deleted.
    /// </exception>
    /// <returns>
    ///     <see cref="UserProfile"/>
    ///     Found user profile.
    /// </returns>
    public Task<UserProfile> GetUserProfileAsync(Guid userProfileId);

    /// <summary>
    ///     Gets user by the given email address.
    /// </summary>
    ///     <param name="emailAddress">Email address to search by.</param>
    /// <exception cref="SiGameReborn.Common.Domain.Exceptions.NotFoundException">
    ///     Is thrown if there is no user profile with such an email address or if this user is soft deleted.
    /// </exception>
    /// <returns>
    ///     <see cref="UserProfile"/>
    ///     Found user profile.
    /// </returns>
    public Task<UserProfile> GetUserProfileByEmailAddressAsync(string emailAddress);

    /// <summary>
    ///     Gets user by the given username.
    /// </summary>
    ///     <param name="userName">User ID to search by.</param>
    /// <exception cref="SiGameReborn.Common.Domain.Exceptions.NotFoundException">
    ///     Is thrown if there is no user profile with such an username or if this user is soft deleted.
    /// </exception>
    /// <returns>
    ///     <see cref="UserProfile"/>
    ///     Found user profile.
    /// </returns>
    public Task<UserProfile> GetUserProfileByUsernameAsync(string userName);

    /// <summary>
    ///     Creates or updates user
    /// </summary>
    /// <param name="userProfile">
    ///     <see cref="UserProfile"/>
    ///     Profile to save
    /// </param>
    /// <exception cref="SiGameReborn.Common.Domain.Exceptions.CoreLogicException">
    ///     Is thrown if user profile was soft deleted.
    /// </exception>
    /// <returns>
    ///     <see cref="UserProfile"/>
    ///     Updated user profile
    /// </returns>
    public Task<UserProfile> SaveUserProfileAsync(UserProfile userProfile);

    /// <summary>
    ///     Removes user by with the given ID.
    /// </summary>
    /// <exception cref="SiGameReborn.Common.Domain.Exceptions.NotFoundException">
    ///     Is thrown when there is no user with the provided ID.
    /// </exception>
    /// <param name="userId">Id of the user.</param>
    public Task SoftDeletedUserAsync(Guid userId);
}