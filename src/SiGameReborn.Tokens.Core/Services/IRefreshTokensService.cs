using SiGameReborn.Tokens.Core.Dtos;
using SiGameReborn.Tokens.Core.Models;

namespace SiGameReborn.Tokens.Core.Services;

public interface IRefreshTokensService
{
    /// <summary>
    ///     Creates new refresh token and saves it to the database.
    /// </summary>
    /// <param name="createRefreshTokenDto">
    ///     Data required to create new refresh token.
    /// </param>
    /// <exception cref="SiGameReborn.Common.Domain.Exceptions.DuplicateException">
    ///     Is thrown when refresh token with the provided IP address and device name already exists.
    /// </exception>
    /// <returns>
    ///    Create refresh token.
    /// </returns>
    public Task<RefreshToken> CreateTokenAsync(CreateRefreshTokenDto createRefreshTokenDto);

    /// <summary>
    ///     Extends token lifetime by updating refresh token ExpiresAt property.
    /// </summary>
    /// <exception cref="SiGameReborn.Common.Domain.Exceptions.NotFoundException">
    ///     Is thrown if token with the provided ID does not exist.
    /// </exception>
    /// <returns>
    ///     Updated refresh token
    /// </returns>
    public Task<RefreshToken> ExtendRefreshTokenLifetimeDto(ExtendRefreshTokenLifetimeDto tokenLifetimeDto);

    public Task<RefreshToken> GetTokenAsync(Guid refreshTokenId);

    public Task<RefreshToken> GetTokenAsync(string token);

    public Task RemoveTokenAsync(string token);

    public Task RemoveTokenAsync(Guid refreshTokenId);
}