using System.ComponentModel.DataAnnotations;

namespace SiGameReborn.Tokens.Core.Dtos;

public record CreateRefreshTokenDto
{
    public Guid UserId { get; set; }

    [Range(1, int.MaxValue)]
    public int TtlMinutes { get; set; }

    [Required]
    public string IpAddress { get; set; } = string.Empty;

    [Required]
    public string DeviceName { get; set; } = string.Empty;
};