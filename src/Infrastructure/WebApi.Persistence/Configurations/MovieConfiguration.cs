using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.OriginalTitle)
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.Property(x => x.Poster)
            .HasMaxLength(500);

        builder.Property(x => x.Banner)
            .HasMaxLength(500);

        builder.Property(x => x.Duration)
            .HasMaxLength(20);

        builder.Property(x => x.Director)
            .HasMaxLength(150);

        builder.Property(x => x.TrailerUrl)
            .HasMaxLength(500);

        builder.Property(x => x.VideoUrl)
            .HasMaxLength(500);

        builder.Property(x => x.ExternalUrl)
            .HasMaxLength(500);

        builder.HasOne(x => x.BookSource)
            .WithMany(x => x.MovieAdaptations)
            .HasForeignKey(x => x.BookSourceId)
            .OnDelete(DeleteBehavior.SetNull); // Book silinsə, Movie qalır amma BookSourceId null olur
    }
}