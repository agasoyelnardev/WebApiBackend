using MediatR;
using WebApi.Application.Features.Admin.Dtos;

namespace WebApi.Application.Features.Admin.Queries.GetAdminStats;

public record GetAdminStatsQuery : IRequest<AdminStatsDto>;