using MediatR;

namespace WebApi.Application.Features.LiveStreams.Commands.CreateLiveStream;

public class CreateLiveStreamCommand : IRequest<Guid>
{
    public string ChannelKey { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StreamUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Movie, Book, Discussion
}