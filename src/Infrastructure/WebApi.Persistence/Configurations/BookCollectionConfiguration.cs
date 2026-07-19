using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

public class BookCollectionConfiguration : IEntityTypeConfiguration<BookCollection>
{
    public void Configure(EntityTypeBuilder<BookCollection> builder)
    {
        builder.HasOne(x => x.User)
            .WithMany(x => x.BookCollections)
            .HasForeignKey(x => x.UserId);
    }
}