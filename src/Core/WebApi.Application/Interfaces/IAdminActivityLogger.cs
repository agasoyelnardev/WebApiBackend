namespace WebApi.Application.Interfaces;

public interface IAdminActivityLogger
{
    Task LogAsync(
        string action,
        string description,
        Guid? targetEntityId = null,
        string? targetEntityType = null,
        CancellationToken cancellationToken = default);
}