using SiGameReborn.User.Core.Models;

namespace SiGameReborn.User.Core.Services;

public interface IUserPasswordService
{
    public Task<UserPassword> GetActiveUserPasswordAsync(Guid userId);

    public Task<ICollection<UserPassword>> GetUserPasswordsAsync(Guid userId);

    /// <summary>
    ///     Adds new user password and marks other user passwords as deleted.
    ///     If there are more than 5 passwords, the oldest ones will be removed.
    /// </summary>
    /// <param name="newPassword">
    ///     New password to be 'pushed'.
    /// </param>
    /// <returns>
    ///     <see cref="UserPassword"/>
    ///     Saved user password model.
    /// </returns>
    public Task<UserPassword> PushUserPasswordAsync(UserPassword newPassword);

    /// <summary>
    ///     Expires user password with the given password hash by marking it as deleted.
    /// </summary>
    /// <param name="passwordHash">Password hash to search by.</param>
    /// <exception cref="SiGameReborn.Common.Domain.Exceptions.NotFoundException">
    ///     Is thrown when there is no password with such a hash.
    /// </exception>
    public Task ExpireUserPasswordAsync(string passwordHash);

    /// <summary>
    ///     Expires user password with the given password ID by marking it as deleted.
    /// </summary>
    ///     <param name="passwordId">Password ID to search by.
    /// </param>
    /// <exception cref="SiGameReborn.Common.Domain.Exceptions.NotFoundException">
    ///     When there is no password with such an ID.
    /// </exception>
    public Task ExpireUserPasswordAsync(Guid passwordId);
}