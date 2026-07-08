using WebApi.Domain.Entities;

namespace WebApi.Application.Interfaces;

public interface IChatRepository
{
    Task<List<ChatMessage>> GetMessagesByRoomIdAsync(Guid roomId);
    Task AddMessageAsync(ChatMessage message);
    Task AddRoomAsync(StreamRoom room);
    Task<bool> SaveChangesAsync();
    
}