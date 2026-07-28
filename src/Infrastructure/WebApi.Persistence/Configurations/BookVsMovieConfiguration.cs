using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class BookVsMovieConfiguration : IEntityTypeConfiguration<BookVsMovie>
{
    public void Configure(EntityTypeBuilder<BookVsMovie> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Book)
            .WithMany()
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Movie)
            .WithMany()
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Votes)
            .WithOne(x => x.BookVsMovie)
            .HasForeignKey(x => x.BookVsMovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}