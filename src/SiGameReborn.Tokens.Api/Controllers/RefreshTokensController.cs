using Microsoft.AspNetCore.Mvc;

using SiGameReborn.Common.Domain.Models.Paging;
using SiGameReborn.Tokens.Core.Dtos;
using SiGameReborn.Tokens.Core.Models;
using SiGameReborn.Tokens.Core.Services;

namespace SiGameReborn.Tokens.Api.Controllers;

[ApiController, Route("api/refresh-tokens")]
public class RefreshTokensController : ControllerBase
{
    private readonly IRefreshTokensService _refreshTokensService;

    public RefreshTokensController(IRefreshTokensService refreshTokensService)
    {
        _refreshTokensService = refreshTokensService;
    }

    [HttpGet("")]
    public async Task<ActionResult<PagedResult<RefreshToken>>> GetRefreshTokensListAsync(
        [FromQuery] PageDescriptor page,
        [FromQuery] RefreshTokensFilter filter,
        [FromServices] IRefreshTokensListService refreshTokensListService
    )
    {
        var tokens = await refreshTokensListService.GetRefreshTokensListAsync(page, filter);
        return Ok(tokens);
    }

    [HttpPost("")]
    public async Task<ActionResult<RefreshToken>> CreateRefreshTokenAsync(
        [FromBody] CreateRefreshTokenDto refreshTokenDto
    )
    {
        var token = await _refreshTokensService.CreateTokenAsync(refreshTokenDto);
        return Ok(token);
    }

    [HttpPatch("")]
    public async Task<ActionResult<RefreshToken>> ExtendTokenLifetimeAsync(
        [FromBody] ExtendRefreshTokenLifetimeDto extendLifetimeDto
    )
    {
        var token = await _refreshTokensService.ExtendRefreshTokenLifetimeDto(extendLifetimeDto);
        return Ok(token);
    }

    [HttpGet("{refreshToken:alpha}")]
    public async Task<ActionResult<RefreshToken>> GetRefreshTokenAsync(string refreshToken)
    {
        var token = await _refreshTokensService.GetTokenAsync(refreshToken);
        return Ok(token);
    }

    [HttpGet("{refreshTokenId:guid}")]
    public async Task<ActionResult<RefreshToken>> GetRefreshTokenAsync(Guid refreshTokenId)
    {
        var token = await _refreshTokensService.GetTokenAsync(refreshTokenId);
        return Ok(token);
    }

    [HttpDelete("{refreshTokenId:guid}")]
    public async Task<NoContentResult> RemoteTokenAsync(Guid refreshTokenId)
    {
        await _refreshTokensService.RemoveTokenAsync(refreshTokenId);
        return NoContent();
    }

    [HttpDelete("{refreshToken:alpha}")]
    public async Task<NoContentResult> RemoteTokenAsync(string refreshToken)
    {
        await _refreshTokensService.RemoveTokenAsync(refreshToken);
        return NoContent();
    }
}