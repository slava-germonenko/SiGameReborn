namespace SiGameReborn.Tokens.Core.Dtos;

public record ExtendRefreshTokenLifetimeDto
{
    public Guid RefreshTokenId { get; set; }

    public int TtlMinutes { get; set; }
}