using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class BookCollectionLikeConfiguration
    : IEntityTypeConfiguration<BookCollectionLike>
{
    public void Configure(EntityTypeBuilder<BookCollectionLike> builder)
    {
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);  

        builder.HasIndex(x => new
        {
            x.UserId,
            x.BookCollectionId
        }).IsUnique();
    }
}