using System.Security.Cryptography;

using Microsoft.EntityFrameworkCore;

using SiGameReborn.Common.Domain.Exceptions;
using SiGameReborn.Common.EntityFramework.Extensions;
using SiGameReborn.Tokens.Application.Configuration;
using SiGameReborn.Tokens.Core;
using SiGameReborn.Tokens.Core.Dtos;
using SiGameReborn.Tokens.Core.Models;
using SiGameReborn.Tokens.Core.Services;

namespace SiGameReborn.Tokens.Application.Services;

public class RefreshTokensService : IRefreshTokensService
{
    private readonly IRefreshTokensConfiguration _refreshTokensConfiguration;

    private readonly TokensContext _context;

    public RefreshTokensService(TokensContext context, IRefreshTokensConfiguration refreshTokensConfiguration)
    {
        _context = context;
        _refreshTokensConfiguration = refreshTokensConfiguration;
    }

    public async Task<RefreshToken> CreateTokenAsync(CreateRefreshTokenDto createRefreshTokenDto)
    {
        await EnsureUserExists(createRefreshTokenDto.UserId);
        var tokenExists = await _context.RefreshTokens.AnyAsync(
            token => token.UserId == createRefreshTokenDto.UserId
                && token.RequestedByDevice == createRefreshTokenDto.DeviceName
                && token.RequestedByIpAddress == createRefreshTokenDto.IpAddress
        );
        if (tokenExists)
        {
            throw new DuplicateException(
                $"Токен для пользователя \"{createRefreshTokenDto.UserId}\" и устройства \"{createRefreshTokenDto.DeviceName}\" c IP адресом \"{createRefreshTokenDto.IpAddress}\" уже существует."
            );
        }

        var newRefreshToken = new RefreshToken
        {
            UserId = createRefreshTokenDto.UserId,
            RequestedByIpAddress = createRefreshTokenDto.IpAddress,
            RequestedByDevice = createRefreshTokenDto.DeviceName,
            Token = GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(createRefreshTokenDto.TtlMinutes),
        };

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        return newRefreshToken;
    }

    public async Task<RefreshToken> ExtendRefreshTokenLifetimeDto(ExtendRefreshTokenLifetimeDto tokenLifetimeDto)
    {
        var token = await GetTokenAsync(tokenLifetimeDto.RefreshTokenId);
        token.ExpiresAt = DateTime.UtcNow.AddMinutes(tokenLifetimeDto.TtlMinutes);
        token.Token = GenerateRefreshToken();
        _context.RefreshTokens.Update(token);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<RefreshToken> GetTokenAsync(Guid refreshTokenId)
    {
        return await _context.RefreshTokens.FindOrNotFoundException(
            refreshTokenId,
            $"Токен с идентификатором \"{refreshTokenId}\" не найден"
        );
    }

    public async Task<RefreshToken> GetTokenAsync(string token)
    {
        return await _context.RefreshTokens.FirstOrNotFoundExceptionAsync(
            t => t.Token == token,
            $"Токен \"{token}\" не найден"
        );
    }

    public async Task RemoveTokenAsync(string token)
    {
        var tokenToRemove = await GetTokenAsync(token);
        _context.RefreshTokens.Remove(tokenToRemove);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveTokenAsync(Guid refreshTokenId)
    {
        var tokenToRemove = await GetTokenAsync(refreshTokenId);
        _context.RefreshTokens.Remove(tokenToRemove);
        await _context.SaveChangesAsync();
    }

    private async Task EnsureUserExists(Guid userId)
    {
        var userExistsAndIsActive = await _context.UserProfiles.AnyAsync(
            user => user.Id == userId && user.DeletedDate == null
        );

        if (!userExistsAndIsActive)
        {
            throw new NotFoundException($"Пользователь с идентификатором {userId} не существует или был удалён.");
        }
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[_refreshTokensConfiguration.RefreshTokenLength];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}