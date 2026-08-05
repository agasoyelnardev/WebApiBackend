using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class LiveStreamMessageConfiguration : IEntityTypeConfiguration<LiveStreamMessage>
{
    public void Configure(EntityTypeBuilder<LiveStreamMessage> builder)
    {
        builder.Property(x => x.Message).IsRequired().HasMaxLength(500);
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);

        builder.HasOne(x => x.LiveStream)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.LiveStreamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.LiveStreamId, x.CreatedAt });
    }
}