using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class NotificationConfiguration
    : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.IsRead)
            .HasDefaultValue(false);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.IsRead);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.RelatedEntityId);
    }
}