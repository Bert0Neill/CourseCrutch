using Microsoft.Extensions.Logging;
using Moq;

namespace CourseCrutch.Tests.TestDoubles;

public static class LoggerMockExtensions
{
    public static void VerifyLogged<T>(this Mock<ILogger<T>> logger, LogLevel level, Times times) =>
        logger.Verify(
            l => l.Log(
                level,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
}
