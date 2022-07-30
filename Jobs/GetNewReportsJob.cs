using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace ProtonDbApi.Jobs;

public static class GetNewReportsJob
{
    public static async Task GetAndExtractReportsFromGithub()
    {
        var now = DateTime.Now;
        var month = now.ToString("MMM").ToLower();
        // var day = now.Day;
        var day = 1;
        var year = now.Year;
        var tarPath = @"Reports/download.tar.gz";
        var tarDir = @"Reports";
        var tarFileName = month + day + "_" + year + ".tar.gz";
        var reportUrl = "https://github.com/bdefore/protondb-data/raw/master/reports/reports_" + tarFileName;

        if (File.Exists(tarPath))
        {
            File.Delete(tarPath);
        }

        if (File.Exists(tarDir + "reports_piiremoved.json"))
        {
            File.Delete(tarPath);
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
            var errorString = "Error fetching tar.gz file from github";
        }
        
        try
        {
            var inStream = File.OpenRead(tarPath);
            var gzipStream = new GZipInputStream(inStream);
            var tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(tarDir);
            tarArchive.Close();
            inStream.Close();
        }
        catch (Exception ex)
        {
            var errorString = "Failed to extract downloaded tar.gz file";
        }
    }
}