using Microsoft.EntityFrameworkCore;

using SiGameReborn.Common.Domain.Exceptions;
using SiGameReborn.Common.EntityFramework.Extensions;
using SiGameReborn.User.Core;
using SiGameReborn.User.Core.Models;
using SiGameReborn.User.Core.Services;

namespace SiGameReborn.User.Application.Services;

public class UserPasswordService : IUserPasswordService
{
    private const int MaxUserPasswordsStored = 5;

    private readonly UserContext _userContext;


    public UserPasswordService(UserContext userContext)
    {
        _userContext = userContext;
    }


    public async Task<UserPassword> GetActiveUserPasswordAsync(Guid userId)
    {
        await EnsureUserExistsAsync(userId);

        return await _userContext.UserPasswords.FirstOrNotFoundExceptionAsync(
            password => password.UserId == userId && password.DeletedDate == null,
            "У пользователя нет активого пароля"
        );
    }

    public async Task<ICollection<UserPassword>> GetUserPasswordsAsync(Guid userId)
    {
        await EnsureUserExistsAsync(userId);

        return await _userContext.UserPasswords.AsNoTracking()
            .Where(password => password.UserId == userId)
            .ToListAsync();
    }

    public async Task<UserPassword> PushUserPasswordAsync(UserPassword newPassword)
    {
        var allUserPasswords = await GetUserPasswordsAsync(newPassword.UserId);
        if (allUserPasswords.Any(password => password.PasswordHash == newPassword.PasswordHash))
        {
            throw new CoreLogicException("Невозможно использовать один из последних пяти паролей.");
        }

        var userPasswordsToRemove = allUserPasswords.OrderByDescending(password => password.CreatedDate)
            .Skip(MaxUserPasswordsStored - 1)
            .ToList();

        if (userPasswordsToRemove.Any())
        {
            _userContext.UserPasswords.RemoveRange(userPasswordsToRemove);
        }

        var userPasswordsToExpire = allUserPasswords.Take(MaxUserPasswordsStored - 1).ToList();
        foreach (var userPassword in userPasswordsToExpire)
        {
            userPassword.MarkAsDeleted();
        }
        _userContext.UserPasswords.UpdateRange(userPasswordsToExpire);

        var userPasswordToAdd = new UserPassword
        {
            PasswordHash = newPassword.PasswordHash,
            UserId = newPassword.UserId
        };

        _userContext.UserPasswords.Add(userPasswordToAdd);
        await _userContext.SaveChangesAsync();

        return userPasswordToAdd;
    }

    public async Task ExpireUserPasswordAsync(string passwordHash)
    {
        var passwordToRemove = await _userContext.UserPasswords.FirstOrNotFoundExceptionAsync(
            password => password.PasswordHash == passwordHash,
            $"Пароль с хешом \"{passwordHash}\" не найден."
        );

        passwordToRemove.MarkAsDeleted();
        _userContext.Update(passwordToRemove);
        await _userContext.SaveChangesAsync();
    }

    public async Task ExpireUserPasswordAsync(Guid passwordId)
    {
        var passwordToRemove = await _userContext.UserPasswords.FirstOrNotFoundExceptionAsync(
            password => password.Id == passwordId,
            $"Пароль с идентификатором \"{passwordId}\" не найден."
        );

        passwordToRemove.MarkAsDeleted();
        _userContext.Update(passwordToRemove);
        await _userContext.SaveChangesAsync();
    }

    private async Task EnsureUserExistsAsync(Guid userId)
    {
        var userExists = await _userContext.UserProfiles.AnyAsync(profile => profile.Id == userId);
        if (!userExists)
        {
            throw new NotFoundException($"Пользователь с иднтификатором \"{userId}\" не найден.");
        }
    }
}