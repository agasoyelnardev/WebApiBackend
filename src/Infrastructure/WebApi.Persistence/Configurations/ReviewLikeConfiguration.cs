using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class ReviewLikeConfiguration : IEntityTypeConfiguration<ReviewLike>
{
    public void Configure(EntityTypeBuilder<ReviewLike> builder)
    {
        builder.HasIndex(x => new { x.UserId, x.ReviewId })
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction); 
        
    }
}