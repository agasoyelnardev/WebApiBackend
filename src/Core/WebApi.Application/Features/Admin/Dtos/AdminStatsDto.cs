namespace WebApi.Application.Features.Admin.Dtos;

public class AdminStatsDto
{
    public int TotalMovies { get; set; }
    public int TotalBooks { get; set; }
    public int TotalUsers { get; set; }
    public int ActiveUsersCount { get; set; }
    public int BlockedUsersCount { get; set; }
    public int ActiveRoomsCount { get; set; }
    public int TotalReviews { get; set; }
    public int TotalBookReviews { get; set; }
    public int TotalDiscussions { get; set; }
    public int PremiumUsersCount { get; set; }
    public int MonthlyPlanUsersCount { get; set; }
    public int YearlyPlanUsersCount { get; set; }
    public decimal VipRevenue { get; set; }
    public decimal TicketRevenue { get; set; } = 0;
    
}

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