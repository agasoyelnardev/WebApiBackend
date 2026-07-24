using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class BookLikeConfiguration : IEntityTypeConfiguration<BookLike>
{
    public void Configure(EntityTypeBuilder<BookLike> builder)
    {
        builder.HasIndex(x => new { x.UserId, x.BookId })
            .IsUnique();
    }
}