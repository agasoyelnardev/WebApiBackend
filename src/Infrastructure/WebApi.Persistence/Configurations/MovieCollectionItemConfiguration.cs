using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class MovieCollectionItemConfiguration : IEntityTypeConfiguration<MovieCollectionItem>
{
    public void Configure(EntityTypeBuilder<MovieCollectionItem> builder)
    {
        builder.HasIndex(x => new { x.MovieCollectionId, x.MovieId })
            .IsUnique();

        builder.HasOne(x => x.MovieCollection)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.MovieCollectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Movie)
            .WithMany(x => x.MovieCollectionItems)
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}