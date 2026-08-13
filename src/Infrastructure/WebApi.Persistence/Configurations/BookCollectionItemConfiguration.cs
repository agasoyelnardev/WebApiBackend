using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class BookCollectionItemConfiguration : IEntityTypeConfiguration<BookCollectionItem>
{
    public void Configure(EntityTypeBuilder<BookCollectionItem> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasIndex(x => new { x.BookCollectionId, x.BookId })
            .IsUnique();

        builder.HasOne(x => x.BookCollection)
            .WithMany(x => x.BookItems)
            .HasForeignKey(x => x.BookCollectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Book)
            .WithMany(x => x.CollectionItems)
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}