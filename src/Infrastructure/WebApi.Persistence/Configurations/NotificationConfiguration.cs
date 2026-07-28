using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}