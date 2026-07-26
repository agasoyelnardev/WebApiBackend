using System.Collections.Concurrent;
using WebApi.Application.Interfaces;

namespace WebApi.Persistence.Services;

public class OnlineUsersTracker : IOnlineUsersTracker
{
    // connectionId -> userId
    private static readonly ConcurrentDictionary<string, string> Connections = new();

    public void AddConnection(string userId, string connectionId)
    {
        Connections[connectionId] = userId;
    }

    public void RemoveConnection(string connectionId)
    {
        Connections.TryRemove(connectionId, out _);
    }

    public int GetOnlineCount()
    {
        // Eyni istifadəçinin bir neçə tab/cihazdan bağlantısı ola bilər — unikal userId sayı
        return Connections.Values.Distinct().Count();
    }
}