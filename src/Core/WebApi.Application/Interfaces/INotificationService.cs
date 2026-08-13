namespace WebApi.Application.Interfaces;

public interface INotificationService
{
    Task NotifyAsync(
        string userId,
        string type,
        string title,
        string description,
        Guid? relatedEntityId = null,
        CancellationToken cancellationToken = default);
}