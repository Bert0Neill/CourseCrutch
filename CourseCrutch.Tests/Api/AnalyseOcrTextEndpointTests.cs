using System.Net;
using System.Net.Http.Json;
using CourseCrutch.API.Contracts;
using CourseCrutch.Application.Ocr;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace CourseCrutch.Tests.Api;

public sealed class AnalyseOcrTextEndpointTests : IDisposable
{
    private readonly Mock<IOcrAnalysisService> _service = new();
    private readonly WebApplicationFactory<Program> _factory;

    public AnalyseOcrTextEndpointTests()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("FileLogging:Enabled", "false");
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IOcrAnalysisService>();
                services.AddSingleton(_service.Object);
            });
        });
    }

    [Fact]
    public async Task Post_ValidText_ReturnsMappedAnalysis()
    {
        _service
            .Setup(s => s.AnalyseAsync("hello world", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OcrAnalysis(2, 11, "answer", ["hello"]));

        var response = await _factory.CreateClient().PostAsJsonAsync("/api/ocr/analyse", new OcrAnalysisRequest("hello world"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<OcrAnalysisResponse>();
        Assert.NotNull(body);
        Assert.Equal(2, body.WordCount);
        Assert.Equal(11, body.CharacterCount);
        Assert.Equal("answer", body.Answer);
        Assert.Equal(["hello"], body.Keywords);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Post_EmptyText_ReturnsBadRequestWithoutCallingService(string? text)
    {
        var response = await _factory.CreateClient().PostAsJsonAsync("/api/ocr/analyse", new OcrAnalysisRequest(text));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        _service.Verify(s => s.AnalyseAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    public void Dispose() => _factory.Dispose();
}
