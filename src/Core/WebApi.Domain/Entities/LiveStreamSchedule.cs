using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class LiveStreamSchedule : BaseEntity
{
    public string ChannelKey { get; set; } = string.Empty;
    public string ProgramTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime AirTime { get; set; }
    public int DurationMinutes { get; set; }
}