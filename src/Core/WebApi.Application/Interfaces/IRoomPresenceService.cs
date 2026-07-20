namespace WebApi.Application.Interfaces;

public interface IRoomPresenceService
{
    void AddConnection(string roomId, string userId, string connectionId);
    string? RemoveConnection(string connectionId, out bool userFullyLeftRoom);
    bool IsUserInRoom(string roomId, string userId);
    string? GetAnyOtherParticipant(string roomId, string excludeUserId);
}