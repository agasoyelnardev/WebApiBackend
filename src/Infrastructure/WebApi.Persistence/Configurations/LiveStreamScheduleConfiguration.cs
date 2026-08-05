using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Configurations;

public class LiveStreamScheduleConfiguration : IEntityTypeConfiguration<LiveStreamSchedule>
{
    public void Configure(EntityTypeBuilder<LiveStreamSchedule> builder)
    {
        builder.Property(x => x.ChannelKey).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ProgramTitle).IsRequired().HasMaxLength(200);

        builder.HasIndex(x => x.AirTime);
    }
}