using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class BookReviewLikeConfiguration : IEntityTypeConfiguration<BookReviewLike>
{
    public void Configure(EntityTypeBuilder<BookReviewLike> builder)
    {
        builder.HasIndex(x => new { x.UserId, x.BookReviewId })
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);  
    }
}