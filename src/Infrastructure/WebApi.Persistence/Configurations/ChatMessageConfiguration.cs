using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.Property(x => x.Username)
            .HasMaxLength(100);

        builder.Property(x => x.UserAvatarUrl)
            .HasMaxLength(500);

        builder.Property(x => x.MessageText)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(x => x.StreamRoom)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.StreamRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}