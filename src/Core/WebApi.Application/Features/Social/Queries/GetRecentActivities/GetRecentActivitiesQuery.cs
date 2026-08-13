using MediatR;
using WebApi.Application.Features.Social.Dtos;

namespace WebApi.Application.Features.Social.Queries.GetRecentActivities;

public record GetRecentActivitiesQuery(int HoursLimit = 2) : IRequest<List<ActivityDto>>;

