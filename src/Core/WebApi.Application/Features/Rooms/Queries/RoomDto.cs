namespace WebApi.Application.Features.Rooms.Queries;

public record RoomDto(
    Guid Id,
    string Title,
    string StreamUrl,
    string Type,
    bool IsLive,
    bool IsPremium,
    int ViewerCount,
    string CoverImageUrl,
    string CreatedByUserId,
    Guid? MovieId
);