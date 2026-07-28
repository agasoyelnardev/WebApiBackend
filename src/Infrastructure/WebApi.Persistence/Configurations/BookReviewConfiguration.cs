using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class BookReviewConfiguration : IEntityTypeConfiguration<BookReview>
{
    public void Configure(EntityTypeBuilder<BookReview> builder)
    {
        builder.Property(x => x.Comment)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasIndex(x => new { x.UserId, x.BookId })
            .IsUnique();

        builder.HasOne(x => x.Book)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.BookReviews)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ReviewLikes)
            .WithOne(x => x.BookReview)
            .HasForeignKey(x => x.BookReviewId)
            .OnDelete(DeleteBehavior.Cascade);  
    }
}