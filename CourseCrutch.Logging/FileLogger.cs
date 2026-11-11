using Microsoft.Extensions.Logging;

namespace CourseCrutch.Logging;

internal sealed class FileLogger(string categoryName, FileLoggerProvider provider, LogLevel minimumLevel) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None && logLevel >= minimumLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        provider.Write(DateTime.Now, logLevel, categoryName, formatter(state, exception), exception);
    }
}
