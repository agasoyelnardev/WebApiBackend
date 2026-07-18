using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Rooms.Commands;

public record CreateRoomCommand(string RoomName, string Type, Guid? MovieId) : IRequest<Guid>
{
    public string CreatedByUserId { get; set; } = string.Empty;
}

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Guid>
{
    private readonly IChatRepository _repository;
    private readonly IAppDbContext _context;

    public CreateRoomCommandHandler(IChatRepository repository, IAppDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.CreatedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.RoomName))
            throw new BadRequestException("Otaq adı boş ola bilməz.");

        if (request.RoomName.Length > 100)
            throw new BadRequestException("Otaq adı maksimum 100 simvol ola bilər.");

        if (request.MovieId.HasValue)
        {
            var movieExists = await _context.Movies.AnyAsync(
                m => m.Id == request.MovieId.Value && !m.IsDeleted, cancellationToken);

            if (!movieExists)
                throw new NotFoundException("Film tapılmadı və ya silinib.");
        }

        var hasActiveRoom = await _repository.HasActiveRoomByUserAsync(request.CreatedByUserId);
        if (hasActiveRoom)
            throw new BadRequestException("Artıq aktiv otağınız var. Yeni otaq yaratmaq üçün əvvəlcə mövcud otağınızı silin.");

        var room = new StreamRoom
        {
            Id = Guid.NewGuid(),
            Title = request.RoomName,
            Type = string.IsNullOrWhiteSpace(request.Type) ? "Movie" : request.Type,
            CreatedByUserId = request.CreatedByUserId,
            MovieId = request.MovieId,
            IsLive = true
        };

        await _repository.AddRoomAsync(room);
        await _repository.SaveChangesAsync();

        return room.Id;
    }
}