namespace SiGameReborn.Tokens.Core.Dtos;

public record RefreshTokensFilter
{
    public Guid? UserId { get; set; }

    public string? DeviceName { get; set; }

    public string? IpAddress { get; set; }
};