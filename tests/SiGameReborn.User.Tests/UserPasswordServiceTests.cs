using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using SiGameReborn.Common.Domain.Exceptions;
using SiGameReborn.User.Application.Services;
using SiGameReborn.User.Core;
using SiGameReborn.User.Core.Models;
using SiGameReborn.User.Core.Services;

namespace SiGameReborn.User.Tests;

[TestFixture, SingleThreaded]
public class UserPasswordServiceTests
{
    private UserContext _userContext;

    private IUserPasswordService _userPasswordService;

    private readonly Guid _testUserId = Guid.NewGuid();

    public UserPasswordServiceTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
        optionsBuilder.UseInMemoryDatabase("core");
        _userContext = new UserContext(optionsBuilder.Options);
        _userPasswordService = new UserPasswordService(_userContext);
    }

    [OneTimeSetUp]
    public void GlobalSetup()
    {
        _userContext.UserProfiles.Add(new ()
        {
            Id = _testUserId,
            Username = "test",
            EmailAddress = "test@test.com",
        });
        _userContext.SaveChanges();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _userContext.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        _userContext.ChangeTracker.Clear();
    }

    [Test, Order(0)]
    public async Task GetAllUserPassword_ShouldRetrieveAllUserPasswords()
    {
        AddDummyPasswordRecord();
        AddDummyPasswordRecord(true);

        var passwords = await _userPasswordService.GetUserPasswordsAsync(_testUserId);
        Assert.AreEqual(2, passwords.Count);
        Assert.IsTrue(passwords.Any(p => p.Deleted));
        Assert.IsTrue(passwords.Any(p => !p.Deleted));
    }

    [Test]
    public async Task AddPasswordsWithSameHash_ShouldNotBeAdded()
    {
        var password = await _userContext.UserPasswords.FirstOrDefaultAsync() ?? AddDummyPasswordRecord();
        var passwordToPush = new UserPassword
        {
            Id = Guid.NewGuid(),
            PasswordHash = password.PasswordHash,
            UserId = password.UserId,
        };

        Assert.CatchAsync<CoreLogicException>(async () =>
        {
            await _userPasswordService.PushUserPasswordAsync(passwordToPush);
        });
    }

    [Test]
    public void GetSoftDeletedPassword_ShouldThrowNotFoundException()
    {
        var testPassword = AddDummyPasswordRecord(true);
        Assert.CatchAsync<NotFoundException>(async () =>
        {
            var expiredPassword = await _userPasswordService.GetActiveUserPasswordAsync(testPassword.Id);
        });
    }

    [Test]
    public async Task GetActivePassword_ShouldRetrievePassword()
    {
        AddDummyPasswordRecord();
        AddDummyPasswordRecord(true);

        var activePassword = await _userPasswordService.GetActiveUserPasswordAsync(_testUserId);

        Assert.IsFalse(activePassword.Deleted);
        Assert.AreEqual(activePassword.UserId, _testUserId);
    }

    [Test]
    public async Task PushPassword_ShouldPushNewAndRemoveOldest()
    {
        const int maxUserPasswordsStored = 5;
        UserPassword latestPassword = new UserPassword();
        for (var index = 0; index < maxUserPasswordsStored; index++)
        {
            latestPassword = AddDummyPasswordRecord();
        }

        var passwordToPush = new UserPassword
        {
            UserId = _testUserId,
            PasswordHash = "test2",
            Id = Guid.NewGuid(),
        };

        var pushedPassword = await _userPasswordService.PushUserPasswordAsync(passwordToPush);
        var userPasswords = await _userContext.UserPasswords.ToListAsync();

        Assert.AreEqual(maxUserPasswordsStored, userPasswords.Count);
        Assert.IsFalse(pushedPassword.Deleted);
        foreach (var password in userPasswords.Where(p => p.Id != pushedPassword.Id))
        {
            Assert.IsTrue(password.Deleted);
        }
        // Latest password was completely removed.
        Assert.IsTrue(userPasswords.All(p => p.Id != latestPassword.Id));
    }

    [Test]
    public async Task ExpirePasswordById_ShouldBeMarkedAsDeleted()
    {
        var passwordToBeExpiredById = AddDummyPasswordRecord();

        await _userPasswordService.ExpireUserPasswordAsync(passwordToBeExpiredById.Id);
        var password = await _userContext.UserPasswords.FindAsync(passwordToBeExpiredById.Id);

        Assert.IsTrue(password!.Deleted);
    }

    [Test]
    public async Task ExpirePasswordByHash_ShouldBeMarkedAsDeleted()
    {
        var passwordToBeExpired = AddDummyPasswordRecord();

        await _userPasswordService.ExpireUserPasswordAsync(passwordToBeExpired.PasswordHash);
        var password = await _userContext.UserPasswords.FindAsync(passwordToBeExpired.Id);

        Assert.IsTrue(password!.Deleted);
    }

    private UserPassword AddDummyPasswordRecord(bool expired = false)
    {
        var passwordId = Guid.NewGuid();
        var testPassword = new UserPassword
        {
            UserId = _testUserId,
            Id = passwordId,
            PasswordHash = passwordId.ToString(),
        };

        if (expired)
        {
            testPassword.MarkAsDeleted();
        }

        _userContext.UserPasswords.Add(testPassword);
        _userContext.SaveChanges();
        _userContext.ChangeTracker.Clear();
        return testPassword;
    }
}