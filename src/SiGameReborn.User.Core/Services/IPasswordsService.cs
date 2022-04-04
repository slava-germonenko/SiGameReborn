namespace SiGameReborn.User.Core.Services;

public interface IPasswordsService
{
    public string GeneratePasswordHashAsync(string password);
}