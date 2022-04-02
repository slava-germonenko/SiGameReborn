using Microsoft.EntityFrameworkCore;

using SiGameReborn.Common.EntityFramework;
using SiGameReborn.Tokens.Core.Models;

namespace SiGameReborn.Tokens.Core;

public class TokensContext : BaseDbContext
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    public TokensContext(DbContextOptions options) : base(options) { }
}