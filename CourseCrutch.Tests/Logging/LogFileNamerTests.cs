using CourseCrutch.Logging;

namespace CourseCrutch.Tests.Logging;

public sealed class LogFileNamerTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"logs-{Guid.NewGuid():N}");

    [Fact]
    public void CreateNext_FirstRunOfDay_UsesDateWithoutSuffix()
    {
        using var stream = LogFileNamer.CreateNext(_directory, "coursecrutch", new DateTime(2026, 11, 11));

        Assert.Equal("coursecrutch-2026-11-11.log", Path.GetFileName(stream.Name));
    }

    [Fact]
    public void CreateNext_FileExistsForDay_IncrementsSuffix()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(Path.Combine(_directory, "coursecrutch-2026-11-11.log"), string.Empty);
        File.WriteAllText(Path.Combine(_directory, "coursecrutch-2026-11-11-2.log"), string.Empty);

        using var stream = LogFileNamer.CreateNext(_directory, "coursecrutch", new DateTime(2026, 11, 11));

        Assert.Equal("coursecrutch-2026-11-11-3.log", Path.GetFileName(stream.Name));
    }

    [Fact]
    public void CreateNext_OtherDayExists_DoesNotIncrement()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(Path.Combine(_directory, "coursecrutch-2026-11-10.log"), string.Empty);

        using var stream = LogFileNamer.CreateNext(_directory, "coursecrutch", new DateTime(2026, 11, 11));

        Assert.Equal("coursecrutch-2026-11-11.log", Path.GetFileName(stream.Name));
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }
    }
}
