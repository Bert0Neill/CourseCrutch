using CourseCrutch.Application.Ocr;
using CourseCrutch.Infrastructure;
using CourseCrutch.Infrastructure.Ocr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseCrutch.Tests.Infrastructure;

public sealed class DependencyInjectionTests
{
    private static IOcrAnalysisService Resolve(string? apiKey)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["GroqAI:ApiKey"] = apiKey })
            .Build();

        var services = new ServiceCollection()
            .AddLogging()
            .AddSingleton<IConfiguration>(configuration)
            .AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IOcrAnalysisService>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AddInfrastructure_NoApiKey_RegistersMockService(string? apiKey)
    {
        Assert.IsType<MockOcrAnalysisService>(Resolve(apiKey));
    }

    [Fact]
    public void AddInfrastructure_ApiKeySet_RegistersGroqService()
    {
        Assert.IsType<GroqOcrAnalysisService>(Resolve("test-key"));
    }
}
