namespace WebApi.Application.Interfaces;

public enum PointAction
{
    WatchMovie,
    AddReview,
    ReadingProgress50,
    ReadingProgress100
}

public interface IPointsService
{
    Task AwardPointsAsync(string userId, PointAction action, CancellationToken cancellationToken = default);
}