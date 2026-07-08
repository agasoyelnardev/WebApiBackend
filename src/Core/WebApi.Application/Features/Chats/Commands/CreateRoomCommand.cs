using MediatR;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Rooms.Commands;

public record CreateRoomCommand(string RoomName) : IRequest<Guid>; // Geriye yaranan otağın ID-sini qaytaracaq

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Guid>
{
    private readonly IChatRepository _repository;

    public CreateRoomCommandHandler(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new StreamRoom 
        { 
            Id = Guid.NewGuid(), // Yeni ID yaradırıq
            // Əgər StreamRoom sinfində başqa vacib sahələr varsa bura əlavə edin (məsələn Name = request.RoomName)
        };
        
        await _repository.AddRoomAsync(room);
        await _repository.SaveChangesAsync();
        
        return room.Id;
    }
}