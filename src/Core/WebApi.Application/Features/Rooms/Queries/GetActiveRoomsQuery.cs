using MediatR;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Rooms.Queries;

public record GetActiveRoomsQuery : IRequest<List<RoomDto>>;

public class GetActiveRoomsQueryHandler
    : IRequestHandler<GetActiveRoomsQuery, List<RoomDto>>
{
    private readonly IChatRepository _repository;
    private readonly IRoomPresenceService _presenceService;

    public GetActiveRoomsQueryHandler(
        IChatRepository repository,
        IRoomPresenceService presenceService)
    {
        _repository = repository;
        _presenceService = presenceService;
    }

    public async Task<List<RoomDto>> Handle(
        GetActiveRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _repository.GetActiveRoomsAsync();

        return rooms.Select(r =>
        {
            var liveViewerCount = _presenceService.GetUserCount(r.Id.ToString());
            return new RoomDto(
                r.Id,
                r.Title,
                r.StreamUrl,
                r.Type,
                r.IsLive,
                r.IsPremium,
                liveViewerCount,
                r.CoverImageUrl,
                r.CreatedByUserId,
                r.MovieId,
                r.Movie?.Title,
                r.Movie?.Description,
                r.Movie?.Poster,
                r.Movie?.TrailerUrl,
                r.Movie?.VideoUrl
            );
        }).ToList();
    }
}