using MediatR;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Rooms.Queries;

public record GetActiveRoomsQuery : IRequest<List<RoomDto>>;

public class GetActiveRoomsQueryHandler
    : IRequestHandler<GetActiveRoomsQuery, List<RoomDto>>
{
    private readonly IChatRepository _repository;

    public GetActiveRoomsQueryHandler(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RoomDto>> Handle(
        GetActiveRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _repository.GetActiveRoomsAsync();

        return rooms.Select(r => new RoomDto(
            r.Id,
            r.Title,
            r.StreamUrl,
            r.Type,
            r.IsLive,
            r.IsPremium,
            r.ViewerCount,
            r.CoverImageUrl,
            r.CreatedByUserId,
            r.MovieId
        )).ToList();
    }
}