using Microsoft.Extensions.Logging;

namespace CourseCrutch.Logging;

public sealed class FileLoggingOptions
{
    public const string SectionName = "FileLogging";

    public bool Enabled { get; set; } = true;

    public string Directory { get; set; } = "logs";

    public string FileNamePrefix { get; set; } = "coursecrutch";

    public LogLevel MinimumLevel { get; set; } = LogLevel.Information;
}
