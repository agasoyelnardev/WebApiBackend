using MediatR;

namespace WebApi.Application.Features.ReadingProgress.Commands.UpdateReadingProgress;

public class UpdateReadingProgressCommand : IRequest
{
    public Guid BookId { get; set; }
    public int PercentageComplete { get; set; }

    public string UserId { get; set; } = string.Empty;
}