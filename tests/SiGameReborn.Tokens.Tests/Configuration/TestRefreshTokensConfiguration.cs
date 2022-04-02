using SiGameReborn.Tokens.Application.Configuration;

namespace SiGameReborn.Tokens.Tests.Configuration;

public class TestRefreshTokensConfiguration : IRefreshTokensConfiguration
{
    public int RefreshTokenLength => 64;
}