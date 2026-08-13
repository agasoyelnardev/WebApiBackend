using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Persistence.Data;

namespace WebApi.Persistence.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _context;

    public ChatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddRoomAsync(
        StreamRoom room,
        CancellationToken cancellationToken = default)
    {
        await _context.StreamRooms.AddAsync(room, cancellationToken);
    }

    public async Task SetViewerCountAsync(
        Guid roomId,
        int count,
        CancellationToken cancellationToken = default)
    {
        var room = await _context.StreamRooms
            .FirstOrDefaultAsync(r => r.Id == roomId, cancellationToken);

        if (room is not null)
        {
            room.ViewerCount = Math.Max(0, count);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task IncrementViewerCountAsync(
        Guid roomId,
        CancellationToken cancellationToken = default)
    {
        var room = await _context.StreamRooms
            .FirstOrDefaultAsync(r => r.Id == roomId, cancellationToken);

        if (room is not null)
        {
            room.ViewerCount++;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DecrementViewerCountAsync(
        Guid roomId,
        CancellationToken cancellationToken = default)
    {
        var room = await _context.StreamRooms
            .FirstOrDefaultAsync(r => r.Id == roomId, cancellationToken);

        if (room is not null && room.ViewerCount > 0)
        {
            room.ViewerCount--;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<StreamRoom?> GetRoomByIdAsync(
        Guid roomId,
        CancellationToken cancellationToken = default)
    {
        return await _context.StreamRooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == roomId, cancellationToken);
    }

    public async Task<List<ChatMessage>> GetMessagesByRoomIdAsync(
        Guid roomId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ChatMessages
            .AsNoTracking()
            .Where(m => m.StreamRoomId == roomId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task DeleteRoomAsync(
        StreamRoom room,
        CancellationToken cancellationToken = default)
    {
        _context.StreamRooms.Remove(room);
        return Task.CompletedTask;
    }

    public async Task<List<StreamRoom>> GetActiveRoomsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.StreamRooms
            .AsNoTracking()
            .Include(r => r.Movie)
            .Where(r => r.IsLive)
            .OrderByDescending(r => r.ViewerCount)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasActiveRoomByUserAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.StreamRooms
            .AsNoTracking()
            .AnyAsync(
                r => r.CreatedByUserId == userId && r.IsLive,
                cancellationToken);
    }

    public async Task AddMessageAsync(
        ChatMessage message,
        CancellationToken cancellationToken = default)
    {
        await _context.ChatMessages.AddAsync(message, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
    public async Task<ChatMessage?> GetMessageByIdAsync(
        Guid messageId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ChatMessages
            .FirstOrDefaultAsync(m => m.Id == messageId, cancellationToken);
    }
}