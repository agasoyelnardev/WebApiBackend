using MediatR;

namespace WebApi.Application.Common.Events;

public class MovieRatingChangedEvent : INotification
{
    public Guid MovieId { get; }

    public MovieRatingChangedEvent(Guid movieId)
    {
        MovieId = movieId;
    }
}