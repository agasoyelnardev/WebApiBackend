using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class MovieCollectionConfiguration
    : IEntityTypeConfiguration<MovieCollection>
{
    public void Configure(EntityTypeBuilder<MovieCollection> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.CoverImageUrl)
            .HasMaxLength(500);

        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.MovieCollections)
            .HasForeignKey(x => x.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.MovieCollection)
            .HasForeignKey(x => x.MovieCollectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}