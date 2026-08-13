using System.Collections.Concurrent;
using WebApi.Application.Interfaces;

namespace WebApi.Persistence.Services;

public class OnlineUsersTracker : IOnlineUsersTracker
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> UserConnections = new();

    public bool AddConnection(string userId, string connectionId)
    {
        var connections = UserConnections.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>());
        var isFirstConnection = connections.IsEmpty;
        connections[connectionId] = 0;
        return isFirstConnection;
    }

    public bool RemoveConnection(string userId, string connectionId)
    {
        if (!UserConnections.TryGetValue(userId, out var connections))
            return false;

        connections.TryRemove(connectionId, out _);

        if (connections.IsEmpty)
        {
            UserConnections.TryRemove(userId, out _);
            return true; 
        }

        return false;
    }

    public int GetOnlineCount()
    {
        return UserConnections.Count;
    }
}