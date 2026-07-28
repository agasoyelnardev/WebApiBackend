using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class WatchHistoryConfiguration : IEntityTypeConfiguration<WatchHistory>
{
    public void Configure(EntityTypeBuilder<WatchHistory> builder)
    {
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