using MediatR;
using WebApi.Application.Features.ReadingProgress.Dtos;

namespace WebApi.Application.Features.ReadingProgress.Queries.GetReadingHistory;

public record GetReadingHistoryQuery(string UserId) : IRequest<List<ReadingProgressDetailDto>>;