using MediatR;

namespace WebApi.Application.Features.ReadingProgress.Queries.GetReadingProgress;

public class GetReadingProgressQuery : IRequest<int>
{
    public Guid BookId { get; set; }
    public string UserId { get; set; } = string.Empty;
}