namespace WebApi.Application.Interfaces;

public interface IOnlineUsersTracker
{
    bool AddConnection(string userId, string connectionId);      
    bool RemoveConnection(string userId, string connectionId);   
    int GetOnlineCount();
}