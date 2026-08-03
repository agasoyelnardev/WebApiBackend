namespace WebApi.Application.Features.Admin.Dtos;

public class AdminActivityLogDto
{
    public Guid Id { get; set; }
    public string AdminUsername { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? TargetEntityType { get; set; }
    public Guid? TargetEntityId { get; set; }
    public DateTime CreatedAt { get; set; }
}