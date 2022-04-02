using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using SiGameReborn.Common.Domain.Models.Paging;
using SiGameReborn.Tokens.Application.Services;
using SiGameReborn.Tokens.Core;
using SiGameReborn.Tokens.Core.Dtos;
using SiGameReborn.Tokens.Core.Models;

namespace SiGameReborn.Tokens.Tests;

public class RefreshTokensListServiceTests
{
    private readonly TokensContext _tokensContext;

    private readonly RefreshTokensListService _refreshTokensListService;

    private readonly Guid _testUserId = Guid.NewGuid();

    private PageDescriptor _defaultPageDescriptor = new PageDescriptor();

    public RefreshTokensListServiceTests()
    {
        var contextOptionsBuilder = new DbContextOptionsBuilder<TokensContext>();
        contextOptionsBuilder.UseInMemoryDatabase("Core");
        _tokensContext = new TokensContext(contextOptionsBuilder.Options);
        _refreshTokensListService = new RefreshTokensListService(_tokensContext);
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _tokensContext.UserProfiles.Add(new UserProfile
        {
            Id = _testUserId,
        });
        _tokensContext.SaveChanges();
    }

    [SetUp]
    public void Setup()
    {
        _tokensContext.ChangeTracker.Clear();
    }

    [Test]
    public async Task GetRefreshTokensList_ShouldReturnCorrectlyFilteredTokens()
    {
        _tokensContext.RefreshTokens.AddRange(
            new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = _testUserId,
                RequestedByIpAddress = "127.0.0.1",
                RequestedByDevice = "iPhone"
            },
            new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = _testUserId,
                RequestedByIpAddress = "127.0.0.2",
                RequestedByDevice = "Macbook Pro"
            },
            new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = _testUserId,
                RequestedByIpAddress = "127.0.0.3",
                RequestedByDevice = "iPad Pro"
            }
        );
        await _tokensContext.SaveChangesAsync();
        _tokensContext.ChangeTracker.Clear();

        var userFilter = new RefreshTokensFilter
        {
            UserId = _testUserId
        };

        var userSearchResult = await _refreshTokensListService.GetRefreshTokensListAsync(
            _defaultPageDescriptor,
            userFilter
        );

        var ipAddressFilter = new RefreshTokensFilter
        {
            IpAddress = "127.0.0.3"
        };

        var ipAddressSearchResult = await _refreshTokensListService.GetRefreshTokensListAsync(
            _defaultPageDescriptor,
            ipAddressFilter
        );

        var deviceFilter = new RefreshTokensFilter
        {
            DeviceName = "iPhone"
        };

        var deviceSearchResult = await _refreshTokensListService.GetRefreshTokensListAsync(
            _defaultPageDescriptor,
            deviceFilter
        );

        Assert.AreEqual(3, userSearchResult.Total);
        Assert.AreEqual(3, userSearchResult.Items.Count);
        Assert.IsTrue(userSearchResult.Items.All(token => token.UserId == _testUserId));

        Assert.AreEqual(1, ipAddressSearchResult.Total);
        Assert.AreEqual(1, ipAddressSearchResult.Items.Count);
        Assert.IsTrue(ipAddressSearchResult.Items.All(token => token.RequestedByIpAddress == "127.0.0.3"));

        Assert.AreEqual(1, deviceSearchResult.Total);
        Assert.AreEqual(1, deviceSearchResult.Items.Count);
        Assert.IsTrue(deviceSearchResult.Items.All(token => token.RequestedByDevice == "iPhone"));
    }
}