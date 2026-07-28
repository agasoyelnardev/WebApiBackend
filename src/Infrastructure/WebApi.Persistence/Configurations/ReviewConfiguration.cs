using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasIndex(x => new { x.UserId, x.MovieId })
            .IsUnique();

        builder.HasOne(x => x.Movie)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ReviewLikes)
            .WithOne(x => x.Review)
            .HasForeignKey(x => x.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}