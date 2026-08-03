using MediatR;
using WebApi.Application.Features.Admin.Dtos;

namespace WebApi.Application.Features.Admin.Queries.GetAdminActivityLogs;

public class GetAdminActivityLogsQuery : IRequest<List<AdminActivityLogDto>>
{
}