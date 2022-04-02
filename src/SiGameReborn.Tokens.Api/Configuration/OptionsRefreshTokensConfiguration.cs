using Microsoft.Extensions.Options;

using SiGameReborn.Tokens.Api.Settings;
using SiGameReborn.Tokens.Application.Configuration;

namespace SiGameReborn.Tokens.Api.Configuration;

public class OptionsRefreshTokensConfiguration : IRefreshTokensConfiguration
{
    private readonly IOptionsSnapshot<Security> _securityOptions;

    public int RefreshTokenLength => _securityOptions.Value.RefreshTokenLength;

    public OptionsRefreshTokensConfiguration(IOptionsSnapshot<Security> securityOptions)
    {
        _securityOptions = securityOptions;
    }
}