using System.Collections.Concurrent;
using WebApi.Application.Interfaces;

namespace WebApi.Persistence.Services;

public class LiveStreamPresenceService : ILiveStreamPresenceService
{
    
    private readonly ConcurrentDictionary<string, string> _connectionToStream = new();

    public void AddConnection(string streamId, string connectionId)
    {
        _connectionToStream[connectionId] = streamId;
    }

    public string? RemoveConnection(string connectionId)
    {
        _connectionToStream.TryRemove(connectionId, out var streamId);
        return streamId;
    }
}