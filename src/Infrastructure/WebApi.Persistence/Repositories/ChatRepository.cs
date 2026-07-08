using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Persistence.Data;

namespace WebApi.Persistence.Repositories;

public class ChatRepository:IChatRepository
{
    public async Task AddRoomAsync(StreamRoom room)
    {
        await _context.StreamRooms.AddAsync(room);
    }

    private readonly AppDbContext _context;

    public ChatRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ChatMessage>> GetMessagesByRoomIdAsync(Guid roomId)
    {
        return await _context.ChatMessages
            .Where(m => m.StreamRoomId == roomId)
            .ToListAsync();
    }
    
    public async Task AddMessageAsync(ChatMessage message)
    {
        await _context.ChatMessages.AddAsync(message);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}