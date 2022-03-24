using Microsoft.EntityFrameworkCore;

using SiGameReborn.Common.EntityFramework;
using SiGameReborn.User.Core.Models;
using SiGameReborn.User.Core.Models.Configuration;

namespace SiGameReborn.User.Core;

public class UserContext : BaseDbContext
{
    public DbSet<UserPassword> UserPasswords => Set<UserPassword>();

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    public UserContext(DbContextOptions<UserContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserProfileEntityConfiguration());
    }
}