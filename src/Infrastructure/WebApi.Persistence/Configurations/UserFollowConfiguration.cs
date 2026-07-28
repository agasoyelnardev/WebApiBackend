using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class UserFollowConfiguration : IEntityTypeConfiguration<UserFollow>
{
    public void Configure(EntityTypeBuilder<UserFollow> builder)
    {
        builder.HasIndex(x => new { x.FollowerId, x.FollowingId })
            .IsUnique();

        builder.HasOne(x => x.Follower)
            .WithMany(x => x.Following)
            .HasForeignKey(x => x.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Following)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}