namespace WebApi.Application.Features.LiveStreams.Dtos;

public class LiveStreamScheduleDto
{
    public Guid Id { get; set; }
    public string ChannelKey { get; set; } = string.Empty;
    public string ProgramTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime AirTime { get; set; }
    public int DurationMinutes { get; set; }
}