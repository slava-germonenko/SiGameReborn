using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiGameReborn.User.Core.Models.Configuration;

public class UserProfileEntityConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.Property(user => user.ProfileImageUrl)
            .HasConversion(
                url => url == null ? null : url.ToString(),
                urlString => urlString == null ? null : new Uri(urlString)
            );
    }
}