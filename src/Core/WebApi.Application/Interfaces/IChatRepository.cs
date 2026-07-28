using WebApi.Domain.Entities;

namespace WebApi.Application.Interfaces;

public interface IChatRepository
{
    Task<List<ChatMessage>> GetMessagesByRoomIdAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task<List<StreamRoom>> GetActiveRoomsAsync(CancellationToken cancellationToken = default);
    Task<bool> HasActiveRoomByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task DeleteRoomAsync(StreamRoom room, CancellationToken cancellationToken = default);
    Task AddMessageAsync(ChatMessage message, CancellationToken cancellationToken = default);
    Task AddRoomAsync(StreamRoom room, CancellationToken cancellationToken = default);
    Task<StreamRoom?> GetRoomByIdAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task IncrementViewerCountAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task DecrementViewerCountAsync(Guid roomId, CancellationToken cancellationToken = default);
}