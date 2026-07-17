using WebApi.Domain.Entities;

namespace WebApi.Application.Interfaces;

public interface IChatRepository
{
    Task<List<ChatMessage>> GetMessagesByRoomIdAsync(Guid roomId);
    Task<List<StreamRoom>> GetActiveRoomsAsync();
    Task<bool> HasActiveRoomByUserAsync(string userId);
    Task DeleteRoomAsync(StreamRoom room);
    Task AddMessageAsync(ChatMessage message);
    Task AddRoomAsync(StreamRoom room);
    Task<StreamRoom?> GetRoomByIdAsync(Guid roomId);
    Task<bool> SaveChangesAsync();

    Task IncrementViewerCountAsync(Guid roomId);
    Task DecrementViewerCountAsync(Guid roomId);
}