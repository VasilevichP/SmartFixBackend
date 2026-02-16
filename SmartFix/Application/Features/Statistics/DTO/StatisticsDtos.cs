namespace SmartFix.Application.Features.Statistics.DTO;

public class GeneralStatsDto
{
    public int NewRequestsCount { get; set; }
    public int ClosedRequestsCount { get; set; }
    public double AverageRating { get; set; } // CSAT
    public double AvgRepairTimeHours { get; set; }
    public List<DateValueDto> RequestsDynamics { get; set; } = new();
    public List<LabelValueDto> StatusDistribution { get; set; } = new();
}

// 2. Услуги и Финансы
public class ServicesStatsDto
{
    public decimal TotalRevenue { get; set; }
    public List<LabelValueDto> RevenueByDeviceType { get; set; } = new();
    public List<LabelValueDto> TopServices { get; set; } = new();
}

// 3. Специалисты
public class SpecialistsStatsDto
{
    public List<SpecialistPerformanceDto> Performance { get; set; } = new();
}

// 4. Клиенты
public class ClientsStatsDto
{
    public int TotalClients { get; set; }
    public int ReturningClientsCount { get; set; }
}

public class DateValueDto
{
    public string Date { get; set; } // "20.10"
    public int Value { get; set; }
}

public class LabelValueDto
{
    public string Label { get; set; }
    public double Value { get; set; }
}

public class SpecialistPerformanceDto
{
    public string Name { get; set; }
    public int ClosedCount { get; set; }
    public int InProgressCount { get; set; }
    public double AvgRepairTime { get; set; } // Личная скорость (часы)
}