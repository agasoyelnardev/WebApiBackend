using MediatR;

namespace WebApi.Application.Common.Events;

public class BookRatingChangedEvent : INotification
{
    public Guid BookId { get; }

    public BookRatingChangedEvent(Guid bookId)
    {
        BookId = bookId;
    }
}