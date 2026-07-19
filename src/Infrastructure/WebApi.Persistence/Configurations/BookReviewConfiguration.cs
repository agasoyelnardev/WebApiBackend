using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

public class BookReviewConfiguration : IEntityTypeConfiguration<BookReview>
{
    public void Configure(EntityTypeBuilder<BookReview> builder)
    {
        builder.HasOne(x => x.Book)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.BookId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.BookReviews)
            .HasForeignKey(x => x.UserId);
    }
}