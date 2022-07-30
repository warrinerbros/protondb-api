namespace ProtonDbApi.Models;

public class QueryParameters
{
    public int? appId { get; set; }
    public string? gameTitle { get; set; }
    public int? startDate { get; set; }
    public int? endDate { get; set; }
    public int page { get; set; } = 1;
    public int pageSize { get; set; } = 100;
}