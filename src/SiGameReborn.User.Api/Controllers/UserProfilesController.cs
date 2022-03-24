using Microsoft.AspNetCore.Mvc;

using SiGameReborn.Common.Domain.Models.Paging;
using SiGameReborn.User.Api.Extensions;
using SiGameReborn.User.Core.Models;
using SiGameReborn.User.Core.Models.Dtos;
using SiGameReborn.User.Core.Services;

namespace SiGameReborn.User.Api.Controllers;

[ApiController, Route("api/users")]
public class UserProfilesController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;

    public UserProfilesController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    [HttpGet("")]
    public async Task<ActionResult<PagedResult<UserProfile>>> GetUserProfilesPage(
        [FromQuery] PageDescriptor page,
        [FromQuery] UserProfilesFilter? filter,
        [FromServices] IUsersListService usersListService
    )
    {
        var usersPage = await usersListService.GetUsersListAsync(page, filter);
        return Ok(usersPage);
    }

    [HttpGet("{userId:guid}/profile")]
    public async Task<ActionResult<UserProfile>> GetUserProfileAsync(Guid userId)
    {
        var userProfile = await _userProfileService.GetUserProfileAsync(userId);
        return Ok(userProfile);
    }

    [HttpGet("{emailOrUsername:alpha}/profile")]
    public async Task<ActionResult<UserProfile>> GetUserProfileAsync(string emailOrUsername)
    {
        var userProfile = await _userProfileService.GetUserProfileByEmailOrUsername(emailOrUsername);
        return Ok(userProfile);
    }

    [HttpPost("")]
    public async Task<ActionResult<UserProfile>> CreateUserProfileAsync(UserProfile userProfile)
    {
        var savedUser = await _userProfileService.SaveUserProfileAsync(userProfile);
        return Ok(savedUser);
    }

    [HttpPut("")]
    public async Task<ActionResult<UserProfile>> SaveUserProfileAsync(UserProfile userProfile)
    {
        var savedUser = await _userProfileService.SaveUserProfileAsync(userProfile);
        return Ok(savedUser);
    }

    [HttpDelete("{userId:guid}")]
    public async Task<NoContentResult> SoftDeleteUserAsync(Guid userId)
    {
        await _userProfileService.SoftDeletedUserAsync(userId);
        return NoContent();
    }
}