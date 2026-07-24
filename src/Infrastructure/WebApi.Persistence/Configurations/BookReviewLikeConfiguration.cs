using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class BookReviewLikeConfiguration
    : IEntityTypeConfiguration<BookReviewLike>
{
    public void Configure(EntityTypeBuilder<BookReviewLike> builder)
    {
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.BookReview)
            .WithMany()
            .HasForeignKey(x => x.BookReviewId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.UserId, x.BookReviewId })
            .IsUnique();
    }
}