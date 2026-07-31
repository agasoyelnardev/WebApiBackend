// GetRecentActivityQuery.cs
using MediatR;
using WebApi.Application.Features.Admin.Dtos;

namespace WebApi.Application.Features.Admin.Queries.GetRecentActivity;

public record GetRecentActivityQuery : IRequest<RecentActivityDto>;