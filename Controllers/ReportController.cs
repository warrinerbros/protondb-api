using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProtonDbApi.Repos;
using ProtonDbApi.Models;
using Newtonsoft.Json;
using TestApi.Services;

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

    [HttpGet("reports/load")]
    public async Task<ActionResult> LoadReports()
    {
        string path = "short_reports.json";
        string json = System.IO.File.ReadAllText(path);
        var specialCharactersRemovedJson = String.Concat(json.Where(c => c < 255));
        var reportList = JsonConvert.DeserializeObject<List<RawReport>>(specialCharactersRemovedJson);
        var parsedReports = reportList.Select(rawReport => new Reports{
            AppId = Int32.Parse(rawReport.App.Steam.AppId),
            Title = rawReport.App.Title,
            AppSelectionMethod = rawReport.Responses.AppSelectionMethod,
            AudioFaults = rawReport.Responses.AudioFaults,
            Duration = rawReport.Responses.Duration,
            Extra = rawReport.Responses.Extra,
            GraphicalFaults = rawReport.Responses.GraphicalFaults,
            InputFaults = rawReport.Responses.InputFaults,
            Installs = rawReport.Responses.Installs,
            IsImpactedByAntiCheat = rawReport.Responses.IsImpactedByAntiCheat,
            IsMultiplayerImportant = rawReport.Responses.IsMultiplayerImportant,
            LocalMultiplayerAttempted = rawReport.Responses.LocalMultiplayerAttempted,
            Notes = rawReport.Responses.Notes,
            OnlineMultiplayerAttempted = rawReport.Responses.OnlineMultiplayerAttempted,
            Opens = rawReport.Responses.Opens,
            PerformanceFaults = rawReport.Responses.PerformanceFaults,
            ProtonVersion = rawReport.Responses.ProtonVersion,
            SaveGameFaults = rawReport.Responses.SaveGameFaults,
            SignificantBugs = rawReport.Responses.SignificantBugs,
            StabilityFaults = rawReport.Responses.StabilityFaults,
            StartsPlay = rawReport.Responses.StartsPlay,
            Type = rawReport.Responses.Type,
            Verdict = rawReport.Responses.Verdict,
            WindowingFaults = rawReport.Responses.WindowingFaults,
            Timestamp = rawReport.Timestamp,
            Cpu = rawReport.SystemInfo.Cpu,
            Gpu = rawReport.SystemInfo.Gpu,
            GpuDriver = rawReport.SystemInfo.GpuDriver,
            Kernel = rawReport.SystemInfo.Kernel,
            Os = rawReport.SystemInfo.Os,
            Ram = rawReport.SystemInfo.Ram
        });

        try
        {
            Context.Reports.AddRange(parsedReports);
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Could not add report insert parsed reports");
            return StatusCode(500);
        }

        return StatusCode(201);
    }
}
