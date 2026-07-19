using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

public class BookCollectionItemConfiguration : IEntityTypeConfiguration<BookCollectionItem>
{
    public void Configure(EntityTypeBuilder<BookCollectionItem> builder)
    {
        builder.HasOne(x => x.BookCollection)
            .WithMany(x => x.BookItems)
            .HasForeignKey(x => x.BookCollectionId);

        builder.HasOne(x => x.Book)
            .WithMany(x => x.CollectionItems)
            .HasForeignKey(x => x.BookId);
    }
}