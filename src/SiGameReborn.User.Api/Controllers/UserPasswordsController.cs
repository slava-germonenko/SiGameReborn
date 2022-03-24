using Microsoft.AspNetCore.Mvc;

using SiGameReborn.Common.Domain.Models.Paging;
using SiGameReborn.User.Core.Models;
using SiGameReborn.User.Core.Services;

namespace SiGameReborn.User.Api.Controllers;

[ApiController, Route("api/users")]
public class UserPasswordsController : ControllerBase
{
    private readonly IUserPasswordService _userPasswordService;

    public UserPasswordsController(IUserPasswordService userPasswordService)
    {
        _userPasswordService = userPasswordService;
    }

    [HttpGet("{userId:guid}/passwords")]
    public async Task<ActionResult<PagedResult<UserPassword>>> GetUserPasswordsAsync(Guid userId)
    {
        var passwords = await _userPasswordService.GetUserPasswordsAsync(userId);
        return Ok(passwords);
    }

    [HttpGet("{userId:guid}/passwords/active")]
    public async Task<ActionResult<UserPassword>> GetActivePasswordAsync(Guid userId)
    {
        var activePassword = await _userPasswordService.GetActiveUserPasswordAsync(userId);
        return Ok(activePassword);
    }

    [HttpPost("{userId:guid}/password")]
    public async Task<ActionResult<UserPassword>> PushPasswordAsync(Guid userId, UserPassword password)
    {
        password.UserId = userId;
        var savedPassword = await _userPasswordService.PushUserPasswordAsync(password);
        return Ok(savedPassword);
    }

    [HttpDelete("passwords/{passwordId:guid}")]
    public async Task<NoContentResult> ExpirePasswordAsync(Guid passwordId)
    {
        await _userPasswordService.ExpireUserPasswordAsync(passwordId);
        return NoContent();
    }

    [HttpDelete("passwords/{passwordHash:alpha}")]
    public async Task<NoContentResult> ExpirePasswordAsync(string passwordHash)
    {
        await _userPasswordService.ExpireUserPasswordAsync(passwordHash);
        return NoContent();
    }
}