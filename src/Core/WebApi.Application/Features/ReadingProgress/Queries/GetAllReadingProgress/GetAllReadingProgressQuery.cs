using MediatR;

namespace WebApi.Application.Features.ReadingProgress.Queries.GetAllReadingProgress;

public record GetAllReadingProgressQuery(string UserId) : IRequest<Dictionary<Guid, int>>;