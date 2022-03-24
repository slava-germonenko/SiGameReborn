using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using SiGameReborn.Common.Domain.Models.Paging;
using SiGameReborn.User.Application.Services;
using SiGameReborn.User.Core;
using SiGameReborn.User.Core.Models;
using SiGameReborn.User.Core.Models.Dtos;
using SiGameReborn.User.Core.Services;

namespace SiGameReborn.User.Tests;

public class UsersListServiceTests
{
    private UserContext _userContext;

    private IUsersListService _usersListService;

    public UsersListServiceTests()
    {
        var contextOptionsBuilder = new DbContextOptionsBuilder<UserContext>();
        contextOptionsBuilder.UseInMemoryDatabase("Core");
        _userContext = new UserContext(contextOptionsBuilder.Options);
        _usersListService = new UsersListService(_userContext);
    }

    [OneTimeSetUp]
    public void GlobalSetup()
    {
        _userContext.UserProfiles.Add(new UserProfile
        {
            Username = "test1",
            EmailAddress = "test1@test.com"
        });
        _userContext.UserProfiles.Add(new UserProfile
        {
            Username = "test2",
            EmailAddress = "test2@test.com"
        });
        _userContext.SaveChanges();
    }

    [SetUp]
    public void Setup()
    {
        _userContext.ChangeTracker.Clear();
    }

    [Test]
    public async Task SearchUsers_ShouldFilteredUsers()
    {
        var defaultPage = new PageDescriptor {Offset = 0, Count = 5};
        var users = await _usersListService.GetUsersListAsync(defaultPage, new UserProfilesFilter
        {
            Search = "test1",
        });

        Assert.AreEqual(1, users.Total);
        Assert.AreEqual(1, users.Items.Count);
        Assert.AreEqual("test1", users.Items.First().Username);
    }
}