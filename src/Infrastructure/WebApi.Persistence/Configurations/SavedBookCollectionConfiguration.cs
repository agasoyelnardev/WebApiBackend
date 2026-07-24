using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class SavedBookCollectionConfiguration
    : IEntityTypeConfiguration<SavedBookCollection>
{
    public void Configure(EntityTypeBuilder<SavedBookCollection> builder)
    {
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.BookCollection)
            .WithMany()
            .HasForeignKey(x => x.BookCollectionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(x => new
        {
            x.UserId,
            x.BookCollectionId
        }).IsUnique();
    }
}