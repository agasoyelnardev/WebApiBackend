using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class UserMovieListConfiguration : IEntityTypeConfiguration<UserMovieList>
{
    public void Configure(EntityTypeBuilder<UserMovieList> builder)
    {
        // Eyni istifadəçi eyni filmi eyni siyahı tipinə (Favorite/Watchlist) bir dəfə əlavə edə bilsin
        builder.HasIndex(x => new { x.UserId, x.MovieId, x.Type })
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Movie)
            .WithMany()
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}