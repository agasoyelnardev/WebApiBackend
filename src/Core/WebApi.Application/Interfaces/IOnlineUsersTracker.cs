namespace WebApi.Application.Interfaces;

public interface IOnlineUsersTracker
{
    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string connectionId);
    int GetOnlineCount();
}