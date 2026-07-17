using MediatR;
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

    public CreateRoomCommandHandler(IChatRepository repository) => _repository = repository;

    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.CreatedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.RoomName))
            throw new BadRequestException("Otaq adı boş ola bilməz.");

        if (request.RoomName.Length > 100)
            throw new BadRequestException("Otaq adı maksimum 100 simvol ola bilər.");

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