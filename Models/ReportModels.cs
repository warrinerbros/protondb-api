using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProtonDbApi.Models;

// This is the model being stored in the database
public class Reports
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }
    
    // App fields
    public int AppId { get; set; }
    public string Title { get; set; }

    // Response fields
    public string? AppSelectionMethod { get; set; }
    public string? AudioFaults { get; set; }
    public string? Duration { get; set; }
    public string? Extra { get; set; }
    public string? GraphicalFaults { get; set; }
    public string? InputFaults { get; set; }
    public string? Installs { get; set; }
    public string? IsImpactedByAntiCheat { get; set; }
    public string? IsMultiplayerImportant { get; set; }
    public string? LocalMultiplayerAttempted { get; set; }
    public Notes? Notes { get; set; } // Foreign key
    public string? OnlineMultiplayerAttempted { get; set; }
    public string? Opens { get; set; }
    public string? PerformanceFaults { get; set; }
    public string? ProtonVersion { get; set; }
    public string? SaveGameFaults { get; set; }
    public string? SignificantBugs { get; set; }
    public string? StabilityFaults { get; set; }
    public string? StartsPlay { get; set; }
    public string? Type { get; set; }
    public string? Verdict { get; set; }
    public string? WindowingFaults { get; set; }

    // Timestamp field
    public int Timestamp { get; set; }

    // System info fields
    public string? Cpu { get; set; }
    public string? Gpu { get; set; }
    public string? GpuDriver { get; set; }
    public string? Kernel { get; set; }
    public string? Os { get; set; }
    public string? Ram { get; set; }
}

// These are user comments about the response fields
// It's used to parse the json and it gets put in the database
public class Notes 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore]
    public int? Id { get; set; }
    public string? AnswerToWhatGame { get; set; }
    public string? AppSelectionMethod { get; set; }
    public string? AudioFaults { get; set; }
    public string? Duration { get; set; }
    public string? Extra { get; set; }
    public string? GraphicalFaults { get; set; }
    public string? InputFaults { get; set; }
    public string? Installs { get; set; }
    public string? IsImpactedByAntiCheat { get; set; }
    public string? IsMultiplayerImportant { get; set; }
    public string? LocalMultiplayerAttempted { get; set; }
    public string? OnlineMultiplayerAttempted { get; set; }
    public string? Opens { get; set; }
    public string? PerformanceFaults { get; set; }
    public string? ProtonVersion { get; set; }
    public string? SaveGameFaults { get; set; }
    public string? SignificantBugs { get; set; }
    public string? StabilityFaults { get; set; }
    public string? StartsPlay { get; set; }
    public string? Type { get; set; }
    public string? Verdict { get; set; }
    public string? WindowingFaults { get; set; }
}

// The following classes are used to parse the raw json
public class RawReport
{
    public App App { get; set; }
    public Responses Responses { get; set; }
    public int Timestamp { get; set; }
    public SystemInfo SystemInfo { get; set; }
}

public class App
{
    public Steam Steam { get; set; }
    public string Title { get; set; }
}

public class Steam
{
    public string AppId { get; set; }
}

public class Responses
{
    public string? AnswerToWhatGame { get; set; }
    public string? AppSelectionMethod { get; set; }
    public string? AudioFaults { get; set; }
    public string? Duration { get; set; }
    public string? Extra { get; set; }
    public string? GraphicalFaults { get; set; }
    public string? InputFaults { get; set; }
    public string? Installs { get; set; }
    public string? IsImpactedByAntiCheat { get; set; }
    public string? IsMultiplayerImportant { get; set; }
    public string? LocalMultiplayerAttempted { get; set; }
    public Notes? Notes { get; set; }
    public string? OnlineMultiplayerAttempted { get; set; }
    public string? Opens { get; set; }
    public string? PerformanceFaults { get; set; }
    public string? ProtonVersion { get; set; }
    public string? SaveGameFaults { get; set; }
    public string? SignificantBugs { get; set; }
    public string? StabilityFaults { get; set; }
    public string? StartsPlay { get; set; }
    public string? Type { get; set; }
    public string? Verdict { get; set; }
    public string? WindowingFaults { get; set; }
}

public class SystemInfo
{
    public string? Cpu { get; set; }
    public string? Gpu { get; set; }
    public string? GpuDriver { get; set; }
    public string? Kernel { get; set; }
    public string? Os { get; set; }
    public string? Ram { get; set; }
}

public class SearchParameters
{
    public string Title { get; set; }
    public string AppId { get; set; }
    public int? Timestamp { get; set; }
}
