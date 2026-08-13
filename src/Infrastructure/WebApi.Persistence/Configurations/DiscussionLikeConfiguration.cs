using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class DiscussionLikeConfiguration : IEntityTypeConfiguration<DiscussionLike>
{
    public void Configure(EntityTypeBuilder<DiscussionLike> builder)
    {
        builder.HasIndex(x => new { x.UserId, x.DiscussionId })
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);  
    }
}