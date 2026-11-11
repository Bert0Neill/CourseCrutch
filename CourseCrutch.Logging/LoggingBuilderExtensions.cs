using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CourseCrutch.Logging;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddFileLogging(this ILoggingBuilder builder, IConfiguration configuration, string basePath)
    {
        var options = new FileLoggingOptions();
        configuration.GetSection(FileLoggingOptions.SectionName).Bind(options);

        if (options.Enabled)
        {
            builder.AddProvider(new FileLoggerProvider(options, basePath));
        }

        return builder;
    }
}
