using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.LiveStreams.Commands.UpdateLiveStream;

public class UpdateLiveStreamCommandHandler : IRequestHandler<UpdateLiveStreamCommand, Unit>
{
    private const int MaxCategoryLength = 50;

    private readonly IAppDbContext _context;

    public UpdateLiveStreamCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLiveStreamCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Başlıq boş ola bilməz.");

        if (string.IsNullOrWhiteSpace(request.StreamUrl))
            throw new BadRequestException("Yayım linki (StreamUrl) boş ola bilməz.");

        if (string.IsNullOrWhiteSpace(request.Category))
            throw new BadRequestException("Kateqoriya boş ola bilməz.");

        if (request.Category.Length > MaxCategoryLength)
            throw new BadRequestException($"Kateqoriya adı maksimum {MaxCategoryLength} simvol ola bilər.");

        var stream = await _context.LiveStreams
                         .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken)
                     ?? throw new NotFoundException("Canlı yayım kanalı tapılmadı.");

        stream.Title = request.Title;
        stream.Description = request.Description;
        stream.StreamUrl = request.StreamUrl;
        stream.ThumbnailUrl = request.ThumbnailUrl;
        stream.Category = request.Category.Trim();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}