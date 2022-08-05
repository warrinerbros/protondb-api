using System.Diagnostics.CodeAnalysis;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProtonDbApi.Models;
using ProtonDbApi.Repos;

namespace ProtonDbApi.Jobs;

public interface IGetNewReportsJob
{
    Task GetAndExtractReportsFromGithub();
}

public class GetNewReportsJob : IGetNewReportsJob
{
    private ILogger<GetNewReportsJob> Logger;
    private ReportContext Context;

    public GetNewReportsJob(ILogger<GetNewReportsJob> logger, ReportContext context)
    {
        Logger = logger;
        Context = context;
    }

    public async Task GetAndExtractReportsFromGithub()
    {
        var now = DateTime.Now;
        var month = now.ToString("MMM").ToLower();
        var day = 1;
        var year = now.Year;
        var tarPath = @"Reports/download.tar.gz";
        var tarDir = @"Reports";
        var tarFileName = month + day + "_" + year + ".tar.gz";
        var reportUrl = "https://github.com/bdefore/protondb-data/raw/master/reports/reports_" + tarFileName;

        // Remove any old report files
        if (File.Exists(tarPath))
        {
            File.Delete(tarPath);
        }
        if (File.Exists(tarDir + "/reports_piiremoved.json"))
        {
            File.Delete(tarDir + "/reports_piiremoved.json");
        }

        var httpClient = new HttpClient();
        try
        {
            using (var stream = await httpClient.GetStreamAsync(reportUrl))
            {
                using (var fileStream = new FileStream(tarPath, FileMode.CreateNew))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,"Error fetching tar.gz file from github");
        }

        try
        {
            var inStream = File.OpenRead(tarPath);
            var gzipStream = new GZipInputStream(inStream);
            var tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(tarDir);
            tarArchive.Close();
            gzipStream.Close();
            inStream.Close();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,"Failed to extract downloaded tar.gz file");
        }

        await LoadReports();
    }
    
    private async Task LoadReports()
    {
        const string path = "Reports/reports_piiremoved.json";
        var json = await File.ReadAllTextAsync(path);
        
        // Remove non-ascii characters
        var specialCharactersRemovedJson = string.Concat(json.Where(c => c < 255));
        
        var reportList = JsonConvert.DeserializeObject<List<RawReport>>(specialCharactersRemovedJson);
        if (reportList != null)
        {
            var parsedReports = reportList.Select(rawReport => new Reports
            {
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
                // Remove any existing rows
                await Context.Database.ExecuteSqlRawAsync("DELETE FROM Reports");
                await Context.Database.ExecuteSqlRawAsync("DELETE FROM Notes");
                
                // Add new rows
                Context.Reports.AddRange(parsedReports);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Could not add report insert parsed reports");
            }
        }
    }
}