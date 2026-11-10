using System.Net.Http.Headers;
using CourseCrutch.Application.Ocr;
using CourseCrutch.Infrastructure.Ocr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseCrutch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var apiKey = configuration["GroqAI:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            services.AddSingleton<IOcrAnalysisService, MockOcrAnalysisService>();
            return services;
        }

        services.AddHttpClient<IOcrAnalysisService, GroqOcrAnalysisService>(client =>
        {
            client.BaseAddress = new Uri("https://api.groq.com/openai/v1/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        });

        return services;
    }
}
