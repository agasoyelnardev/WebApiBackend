namespace WebApi.Application.Features.Admin.Dtos;

public class AdminStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalMovies { get; set; }
    public int TotalBooks { get; set; }
    public int TotalReviews { get; set; }
    public int TotalBookReviews { get; set; }
    public int TotalDiscussions { get; set; }
    public int PremiumUsersCount { get; set; }
    public int ActiveRoomsCount { get; set; }

    public int MonthlyPlanUsersCount { get; set; }  
    public int YearlyPlanUsersCount { get; set; }    
    public decimal EstimatedRevenue { get; set; }   
}