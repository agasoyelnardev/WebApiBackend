using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.LiveStreams.Commands.CreateLiveStream;

public class CreateLiveStreamCommandHandler : IRequestHandler<CreateLiveStreamCommand, Guid>
{
    private const int MaxCategoryLength = 50;

    private readonly IAppDbContext _context;

    public CreateLiveStreamCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateLiveStreamCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelKey))
            throw new BadRequestException("Kanal açarı (ChannelKey) boş ola bilməz.");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Başlıq boş ola bilməz.");

        if (string.IsNullOrWhiteSpace(request.StreamUrl))
            throw new BadRequestException("Yayım linki (StreamUrl) boş ola bilməz.");

        if (string.IsNullOrWhiteSpace(request.Category))
            throw new BadRequestException("Kateqoriya boş ola bilməz.");

        if (request.Category.Length > MaxCategoryLength)
            throw new BadRequestException($"Kateqoriya adı maksimum {MaxCategoryLength} simvol ola bilər.");

        var normalizedKey = request.ChannelKey.Trim().ToLower();

        var exists = await _context.LiveStreams
            .AnyAsync(s => s.ChannelKey == normalizedKey, cancellationToken); 

        if (exists)
            throw new ConflictException("Bu ChannelKey ilə artıq bir kanal mövcuddur.");

        var stream = new LiveStream
        {
            ChannelKey = normalizedKey,
            Title = request.Title,
            Description = request.Description,
            StreamUrl = request.StreamUrl,
            ThumbnailUrl = request.ThumbnailUrl,
            Category = request.Category.Trim(),
            IsLive = false,
            ViewerCount = 0
        };

        await _context.LiveStreams.AddAsync(stream, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return stream.Id;
    }
}