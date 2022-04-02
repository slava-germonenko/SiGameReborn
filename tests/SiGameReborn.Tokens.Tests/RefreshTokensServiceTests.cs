using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using SiGameReborn.Common.Domain.Exceptions;
using SiGameReborn.Tokens.Application.Services;
using SiGameReborn.Tokens.Core;
using SiGameReborn.Tokens.Core.Dtos;
using SiGameReborn.Tokens.Core.Models;
using SiGameReborn.Tokens.Tests.Configuration;

namespace SiGameReborn.Tokens.Tests;

[TestFixture]
public class RefreshTokensServiceTests
{
    private readonly TokensContext _tokensContext;

    private readonly RefreshTokensService _refreshTokensService;

    private readonly Guid _testUserId = Guid.NewGuid();

    public RefreshTokensServiceTests()
    {
        var contextOptionsBuilder = new DbContextOptionsBuilder<TokensContext>();
        contextOptionsBuilder.UseInMemoryDatabase("Core");
        _tokensContext = new TokensContext(contextOptionsBuilder.Options);
        _refreshTokensService = new RefreshTokensService(
            _tokensContext,
            new TestRefreshTokensConfiguration()
        );
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

    [Test, Order(0)]
    public async Task CreateNewRefreshToken_TokenShouldBeCreated()
    {
        var token = await _refreshTokensService.CreateTokenAsync(new()
        {
            UserId = _testUserId,
            TtlMinutes = 10,
            DeviceName = "Test",
            IpAddress = "127.0.0.1"
        });

        Assert.AreEqual("Test", token.RequestedByDevice);
        Assert.AreEqual("127.0.0.1", token.RequestedByIpAddress);
        Assert.Less(DateTime.UtcNow.AddMinutes(9), token.ExpiresAt);
    }

    [Test, Order(1)]
    public void CreateRefreshTokenForRemovedUser_ShouldThrowException()
    {
        Assert.CatchAsync<NotFoundException>(async () =>
        {
            await _refreshTokensService.CreateTokenAsync(new()
            {
                UserId = Guid.NewGuid(),
                TtlMinutes = 10,
                DeviceName = "Test",
                IpAddress = "127.0.0.1"
            });
        });
    }

    [Test, Order(2)]
    public void CreateTokenThatAlreadyExist_ShouldTrowDuplicateError()
    {
        var tokeDto = new CreateRefreshTokenDto
        {
            UserId = _testUserId,
            TtlMinutes = 10,
            DeviceName = "Test",
            IpAddress = "127.0.0.1"
        };

        Assert.CatchAsync<DuplicateException>(async () =>
        {
            await _refreshTokensService.CreateTokenAsync(tokeDto);
        });
    }

    [Test, Order(3)]
    public async Task ExtendTokenLifetime_ShouldGenerateNewToken()
    {
        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Token = "test1"
        };
        _tokensContext.RefreshTokens.Add(token);
        await _tokensContext.SaveChangesAsync();
        _tokensContext.ChangeTracker.Clear();

        var extendTokenDto = new ExtendRefreshTokenLifetimeDto
        {
            RefreshTokenId = token.Id,
            TtlMinutes = 10,
        };

        await _refreshTokensService.ExtendRefreshTokenLifetimeDto(extendTokenDto);

        var updatedToken = await _tokensContext.RefreshTokens.FindAsync(token.Id);

        Assert.AreEqual(updatedToken!.Id, token.Id);
        Assert.AreNotEqual(updatedToken.Token, token.Token);
    }

    [Test, Order(4)]
    public async Task ExpireToken_ShouldRemoveToken()
    {
        var token1 = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Token = "test1"
        };
        var token2 = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Token = "test2"
        };
        _tokensContext.RefreshTokens.AddRange(token1, token2);
        await _tokensContext.SaveChangesAsync();
        _tokensContext.ChangeTracker.Clear();

        await _refreshTokensService.RemoveTokenAsync(token1.Token);
        await _refreshTokensService.RemoveTokenAsync(token2.Id);

        Assert.IsNull(
            await _tokensContext.RefreshTokens.FindAsync(token1.Id)
        );

        Assert.IsNull(
            await _tokensContext.RefreshTokens.FindAsync(token2.Id)
        );
    }
}