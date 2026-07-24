using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class SavedMovieCollectionConfiguration
    : IEntityTypeConfiguration<SavedMovieCollection>
{
    public void Configure(EntityTypeBuilder<SavedMovieCollection> builder)
    {
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.MovieCollection)
            .WithMany()
            .HasForeignKey(x => x.MovieCollectionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(x => new
        {
            x.UserId,
            x.MovieCollectionId
        }).IsUnique();
    }
}