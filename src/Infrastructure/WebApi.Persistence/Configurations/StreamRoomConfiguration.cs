using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class StreamRoomConfiguration : IEntityTypeConfiguration<StreamRoom>
{
    public void Configure(EntityTypeBuilder<StreamRoom> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.StreamUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.CoverImageUrl)
            .HasMaxLength(500);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Movie)
            .WithMany()
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.Messages)
            .WithOne(x => x.StreamRoom)
            .HasForeignKey(x => x.StreamRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}