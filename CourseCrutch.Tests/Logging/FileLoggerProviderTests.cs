using CourseCrutch.Logging;
using Microsoft.Extensions.Logging;

namespace CourseCrutch.Tests.Logging;

public sealed class FileLoggerProviderTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"logs-{Guid.NewGuid():N}");

    [Fact]
    public void Log_WritesEntryWithLevelAndCategory()
    {
        var options = new FileLoggingOptions { Directory = _directory, FileNamePrefix = "test" };

        string filePath;
        using (var provider = new FileLoggerProvider(options, basePath: _directory))
        {
            provider.CreateLogger("Tests.Sample").LogInformation("hello");
            filePath = provider.FilePath;
        }

        var text = File.ReadAllText(filePath);

        Assert.Contains("[Information] Tests.Sample: hello", text);
    }

    [Fact]
    public void Log_BelowMinimumLevel_IsNotWritten()
    {
        var options = new FileLoggingOptions
        {
            Directory = _directory,
            FileNamePrefix = "test",
            MinimumLevel = LogLevel.Warning
        };

        string filePath;
        using (var provider = new FileLoggerProvider(options, basePath: _directory))
        {
            provider.CreateLogger("Tests.Sample").LogInformation("hidden");
            filePath = provider.FilePath;
        }

        var text = File.ReadAllText(filePath);

        Assert.DoesNotContain("hidden", text);
    }

    [Fact]
    public void Log_RelativeDirectory_ResolvesAgainstBasePath()
    {
        var options = new FileLoggingOptions { Directory = "logs", FileNamePrefix = "test" };

        using var provider = new FileLoggerProvider(options, basePath: _directory);

        Assert.StartsWith(Path.Combine(_directory, "logs"), provider.FilePath);
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }
    }
}
