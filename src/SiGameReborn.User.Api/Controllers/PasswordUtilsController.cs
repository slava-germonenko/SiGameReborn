using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using SiGameReborn.User.Core.Services;

namespace SiGameReborn.User.Api.Controllers;

[ApiController, Route("api/passwords")]
public class PasswordUtilsController : ControllerBase
{
    [HttpGet("hash")]
    public ActionResult<string> GeneratePasswordHashAsync(
        [FromQuery, Required] string password,
        [FromServices] IPasswordsService passwordsService
    )
    {
        var passwordHash = passwordsService.GeneratePasswordHashAsync(password);
        return Ok(passwordHash);
    }
}