using System.Security.Cryptography;

using Microsoft.Extensions.Options;

using SiGameReborn.User.Api.Options;
using SiGameReborn.User.Application.Services.Configuration;

namespace SiGameReborn.User.Api.Configuration;

public class OptionsPasswordsConfiguration : IPasswordsConfiguration
{
    private readonly IOptionsSnapshot<Passwords> _passwordsOptions;

    public HashAlgorithm CreateHashAlgorithm() => GetHashAlgorithmFromOptions();

    public OptionsPasswordsConfiguration(IOptionsSnapshot<Passwords> passwordsOptions)
    {
        _passwordsOptions = passwordsOptions;
    }

    private HashAlgorithm GetHashAlgorithmFromOptions()
    {
        return _passwordsOptions.Value.HashAlgorithmName.ToUpper() switch
        {
            "MD5" => MD5.Create(),
            "SHA512" => SHA512.Create(),
            _  => SHA256.Create()
        };
    }
}