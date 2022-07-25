using Microsoft.EntityFrameworkCore;
using ProtonDbApi.Repos;
using ProtonDbApi.Models;

namespace TestApi.Services;

public interface IReportService
{
    Task<IQueryable<Reports>> GetReports(QueryParameters queryParameters);
}

public class ReportService : IReportService
{
    private readonly ILogger<ReportService> Logger;
    private readonly ReportContext Context;

    public ReportService(ILogger<ReportService> logger, ReportContext context)
    {
        Logger = logger;
        Context = context;
    }

    public async Task<IQueryable<Reports>> GetReports(QueryParameters queryParameters)
    {
        var reports = Context.Reports.Select(a => a);

        if (queryParameters.appId is not null)
        {
            reports = reports.Where(b => b.AppId == queryParameters.appId);
        }

        if (queryParameters.gameTitle is not null)
        {
            reports = reports.Where(b => b.Title.Contains(queryParameters.gameTitle));
        }
        
        if (queryParameters.startDate is not null)
        {
            reports = reports.Where(b => b.Timestamp > queryParameters.startDate);
        }
        
        if (queryParameters.endDate is not null)
        {
            reports = reports.Where(b => b.Timestamp < queryParameters.endDate);
        }
        
        return reports
            .OrderBy(x => x.Title)
            .Skip((queryParameters.page - 1) * queryParameters.pageSize)
            .Take(queryParameters.pageSize)
            .Include(s => s.Notes);
    }
}