using System.Text;

using SiGameReborn.User.Application.Services.Configuration;
using SiGameReborn.User.Core.Services;

namespace SiGameReborn.User.Application.Services;

public class PasswordsService : IPasswordsService
{
    private readonly IPasswordsConfiguration _passwordsConfiguration;

    public PasswordsService(IPasswordsConfiguration passwordsConfiguration)
    {
        _passwordsConfiguration = passwordsConfiguration;
    }

    public string GeneratePasswordHashAsync(string password)
    {
        using var hasher = _passwordsConfiguration.CreateHashAlgorithm();
        var hashBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashBytes);
    }
}