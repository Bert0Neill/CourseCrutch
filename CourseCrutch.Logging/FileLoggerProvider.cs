using Microsoft.Extensions.Logging;

namespace CourseCrutch.Logging;

public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly StreamWriter _writer;
    private readonly LogLevel _minimumLevel;
    private readonly object _lock = new();
    private bool _disposed;

    public FileLoggerProvider(FileLoggingOptions options, string basePath)
    {
        var directory = Path.IsPathRooted(options.Directory)
            ? options.Directory
            : Path.Combine(basePath, options.Directory);

        var stream = LogFileNamer.CreateNext(directory, options.FileNamePrefix, DateTime.Now);

        _writer = new StreamWriter(stream) { AutoFlush = true };
        _minimumLevel = options.MinimumLevel;
    }

    public string FilePath => ((FileStream)_writer.BaseStream).Name;

    public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, this, _minimumLevel);

    internal void Write(DateTime timestamp, LogLevel logLevel, string categoryName, string message, Exception? exception)
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            _writer.WriteLine($"{timestamp:yyyy-MM-dd HH:mm:ss.fff} [{logLevel}] {categoryName}: {message}");

            if (exception is not null)
            {
                _writer.WriteLine(exception);
            }
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _writer.Dispose();
        }
    }
}
