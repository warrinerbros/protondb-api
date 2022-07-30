using ProtonDbApi.Jobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProtonDbApi.Repos;
using ProtonDbApi.Models;
using Newtonsoft.Json;
using ProtonDbApi.Services;

namespace ProtonDbApi.Controllers;

[ApiController]
[Route("api/")]
public class ReportController : ControllerBase
{
    private readonly ILogger<ReportController> Logger;
    private readonly ReportContext Context;
    private IReportService ReportService;

    public ReportController(ILogger<ReportController> logger, ReportContext context, IReportService reportService)
    {
        Logger = logger;
        Context = context;
        ReportService = reportService;
    }
    
    [HttpGet("reports/")]
    public async Task<ActionResult<IEnumerable<Reports>>> GetReports([FromQuery] QueryParameters queryParameters)
    {
        if (queryParameters.pageSize is < 1 or > 100)
        {
            queryParameters.pageSize = 100;
        }

        var reportsQuery = await ReportService.GetReports(queryParameters);

        try
        {
            return await reportsQuery.ToListAsync();
        }
        catch (Exception ex)
        {
            const string errorString = "An error occured while processing the request";
            Logger.LogError(errorString);
            return Problem( title: errorString, detail: ex.Message);
        }
    }
}
