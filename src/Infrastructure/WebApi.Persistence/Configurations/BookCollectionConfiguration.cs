using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class BookCollectionConfiguration : IEntityTypeConfiguration<BookCollection>
{
    public void Configure(EntityTypeBuilder<BookCollection> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.Cover)
            .HasMaxLength(500);

        builder.HasOne(x => x.User)
            .WithMany(x => x.BookCollections)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.BookItems)
            .WithOne(x => x.BookCollection)
            .HasForeignKey(x => x.BookCollectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Likes)
            .WithOne(x => x.BookCollection)
            .HasForeignKey(x => x.BookCollectionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}