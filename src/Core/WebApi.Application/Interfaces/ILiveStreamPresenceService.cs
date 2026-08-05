namespace WebApi.Application.Interfaces;

public interface ILiveStreamPresenceService
{
    void AddConnection(string streamId, string connectionId);
    string? RemoveConnection(string connectionId);
}