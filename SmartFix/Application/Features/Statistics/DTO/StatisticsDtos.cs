namespace SmartFix.Application.Features.Statistics.DTO;

public class RequestsStatsDto
{
    public int TotalRequests { get; set; }
    public int ClosedRequests { get; set; }
    public int CancelledRequests { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageCheck { get; set; }
    public double AverageRepairTimeHours { get; set; }
    
    public Dictionary<string, int> RequestsByDay { get; set; } = new();
    public Dictionary<string, int> RequestsByStatus { get; set; } = new();
    public Dictionary<string, int> RequestsByType { get; set; } = new();
    public Dictionary<string, int> RequestsByDeviceType { get; set; } = new();
}

public class ClientsStatsDto    
{
    public int NewClientsCount { get; set; }
    public int ReturningClientRequestsCount { get; set; }
    public double AverageRating { get; set; }
    
    public Dictionary<int, int> RatingDistribution { get; set; } = new();
}

public class MastersStatsDto
{
    public int ActiveMastersCount { get; set; }
    public string TopMasterName { get; set; } = string.Empty;
    public double AverageDiagnosticTimeHours { get; set; }

    public Dictionary<string, decimal> RevenueByMaster { get; set; } = new();
    public Dictionary<string, double> RejectionRateByMaster { get; set; } = new();
}