using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasOne(m => m.StreamRoom)
            .WithMany(r => r.Messages)
            .HasForeignKey(m => m.StreamRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}