using CourseCrutch.Application.Ocr;
using CourseCrutch.Infrastructure.Ocr;
using Microsoft.Extensions.DependencyInjection;

namespace CourseCrutch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IOcrAnalysisService, MockOcrAnalysisService>();

        return services;
    }
}
