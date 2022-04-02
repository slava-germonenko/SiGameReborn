using System.ComponentModel.DataAnnotations;
using SiGameReborn.Common.Domain.Models;

namespace SiGameReborn.Tokens.Core.Models;

public class RefreshToken : BaseEntity
{
    [Required]
    public string RequestedByIpAddress { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string RequestedByDevice { get; set; } = string.Empty;

    public Guid UserId { get; set; }

    [Required, StringLength(300)]
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}