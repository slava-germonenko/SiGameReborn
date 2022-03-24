using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using SiGameReborn.Common.Domain.Exceptions;
using SiGameReborn.User.Application.Services;
using SiGameReborn.User.Core;
using SiGameReborn.User.Core.Models;

namespace SiGameReborn.User.Tests;

[TestFixture]
public class UserProfileServiceTests
{
    private readonly UserContext _userContext;

    private readonly UserProfileService _userProfileService;

    private readonly Guid _softDeletedUserId = Guid.NewGuid();

    private readonly Guid _activeUserId = Guid.NewGuid();

    public UserProfileServiceTests()
    {
        var contextOptionsBuilder = new DbContextOptionsBuilder<UserContext>().UseInMemoryDatabase("core");
        _userContext = new UserContext(contextOptionsBuilder.Options);
        _userProfileService = new UserProfileService(_userContext);
    }

    [OneTimeSetUp]
    public void GlobalSetup()
    {
        var activeUser = new UserProfile
        {
            Id = _activeUserId,
            Username = "active-user",
            EmailAddress = "active-user@test.com"
        };
        var softDeletedUser = new UserProfile
        {
            Id = _softDeletedUserId,
            Username = "soft-deleted-user",
            EmailAddress = "soft-deleted-user@test.com"
        };
        softDeletedUser.MarkAsDeleted();

        _userContext.AddRange(activeUser, softDeletedUser);
        _userContext.SaveChanges();
        _userContext.ChangeTracker.Clear();
    }

    [SetUp]
    public void Setup()
    {
        _userContext.ChangeTracker.Clear();
    }

    [Test]
    public async Task GetActiveUser_ShouldRetrieveUser()
    {
        var activeUser = _userContext.UserProfiles.Find(_activeUserId);

        var foundById = await _userProfileService.GetUserProfileAsync(_activeUserId);
        var foundByEmailAddress = await _userProfileService.GetUserProfileByEmailAddressAsync(activeUser!.EmailAddress);
        var foundByUsername = await _userProfileService.GetUserProfileByUsernameAsync(activeUser.Username);

        Assert.AreEqual(_activeUserId, foundById.Id);
        Assert.AreEqual(activeUser.EmailAddress, foundByEmailAddress.EmailAddress);
        Assert.AreEqual(activeUser.Username, foundByUsername.Username);
        Assert.IsFalse(foundById.Deleted);
        Assert.IsFalse(foundByEmailAddress.Deleted);
        Assert.IsFalse(foundByUsername.Deleted);
    }

    [Test]
    public void GetSoftDeletedUser_ShouldThrowNotFoundException()
    {
        var softDeletedUser = _userContext.UserProfiles.Find(_softDeletedUserId);

        Assert.CatchAsync<NotFoundException>(async () =>
            await _userProfileService.GetUserProfileAsync(_softDeletedUserId)
        );
        Assert.CatchAsync<NotFoundException>(async () =>
            await _userProfileService.GetUserProfileByUsernameAsync(softDeletedUser!.Username)
        );
        Assert.CatchAsync<NotFoundException>(async () =>
            await _userProfileService.GetUserProfileByEmailAddressAsync(softDeletedUser!.EmailAddress)
        );
    }

    [Test]
    public void SavePreviouslyDeletedUser_ShouldThrownException()
    {
        var softDeletedUser = _userContext.UserProfiles.Find(_softDeletedUserId);
        Assert.CatchAsync<CoreLogicException>(async () =>
            await _userProfileService.SaveUserProfileAsync(softDeletedUser!)
        );
    }

    [Test]
    public async Task SavePreviouslyCreateUser_ShouldBeUpdated()
    {
        const string newUsername = "new-active-user";
        const string newEmailAddress = "new-active-user@test.com";
        var activeUser = await _userContext.UserProfiles.FindAsync(_activeUserId);
        _userContext.ChangeTracker.Clear();
        activeUser!.Username = newUsername;
        activeUser.EmailAddress = newEmailAddress;

        var savedUser = await _userProfileService.SaveUserProfileAsync(activeUser);
        Assert.AreEqual(newUsername, savedUser.Username);
        Assert.AreEqual(newEmailAddress, savedUser.EmailAddress);
    }

    [Test]
    public void SaveDuplicateUser_ShouldThrowException()
    {
        var activeUser = _userContext.UserProfiles.Find(_activeUserId);

        Assert.CatchAsync<DuplicateException>(async () =>
        {
            var newUser = new UserProfile
            {
                EmailAddress = "test@test.com",
                Username = activeUser!.Username
            };
            await _userProfileService.SaveUserProfileAsync(newUser);
        });

        Assert.CatchAsync<DuplicateException>(async () =>
        {
            var newUser = new UserProfile
            {
                EmailAddress = activeUser!.EmailAddress,
                Username = "test"
            };
            await _userProfileService.SaveUserProfileAsync(newUser);
        });
    }
}