namespace WebApi.Application.Features.Social.Queries;

public record UserPreviewDto(
    string Id,
    string UserName,
    string Avatar
);