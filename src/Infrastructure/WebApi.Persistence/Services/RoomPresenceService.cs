using System.Collections.Concurrent;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Services;

public class RoomPresenceService : IRoomPresenceService
{
    // roomId -> (connectionId -> userId)
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> Rooms = new();

    // connectionId -> roomId (sürətli axtarış üçün)
    private static readonly ConcurrentDictionary<string, string> ConnectionToRoom = new();

    public void AddConnection(string roomId, string userId, string connectionId)
    {
        var room = Rooms.GetOrAdd(roomId, _ => new ConcurrentDictionary<string, string>());
        room[connectionId] = userId;
        ConnectionToRoom[connectionId] = roomId;
    }

    public string? RemoveConnection(string connectionId, out bool userFullyLeftRoom)
    {
        userFullyLeftRoom = false;

        if (!ConnectionToRoom.TryRemove(connectionId, out var roomId))
            return null;

        if (!Rooms.TryGetValue(roomId, out var room))
            return roomId;

        room.TryRemove(connectionId, out var userId);

        // İstifadəçinin bu otaqda başqa bağlantısı (başqa tab) qalıb-qalmadığını yoxla
        var hasOtherConnections = room.Values.Contains(userId);
        userFullyLeftRoom = !hasOtherConnections;

        if (room.IsEmpty)
            Rooms.TryRemove(roomId, out _);

        return roomId;
    }

    public bool IsUserInRoom(string roomId, string userId)
    {
        return Rooms.TryGetValue(roomId, out var room) && room.Values.Contains(userId);
    }

    public string? GetAnyOtherParticipant(string roomId, string excludeUserId)
    {
        if (!Rooms.TryGetValue(roomId, out var room))
            return null;

        return room.Values.FirstOrDefault(u => u != excludeUserId);
    }
}