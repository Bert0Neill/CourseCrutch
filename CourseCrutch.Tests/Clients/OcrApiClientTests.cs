using System.Net;
using System.Text.Json;
using CourseCrutch.Clients.Services;
using CourseCrutch.Tests.TestDoubles;
using Microsoft.Extensions.Logging;
using Moq;

namespace CourseCrutch.Tests.Clients;

public sealed class OcrApiClientTests
{
    private const string BaseAddress = "http://localhost:5059/";

    private readonly MockHttpHandler _http = new();
    private readonly Mock<ILogger<OcrApiClient>> _logger = new();

    private OcrApiClient CreateClient() => new(_http.CreateClient(BaseAddress), _logger.Object);

    [Fact]
    public async Task AnalyseAsync_AnyText_PostsTextToAnalyseEndpoint()
    {
        _http.Respond(HttpStatusCode.OK, """{"wordCount":1,"characterCount":5,"answer":"a","keywords":[]}""");

        await CreateClient().AnalyseAsync("hello");

        Assert.Equal(HttpMethod.Post, _http.LastRequest!.Method);
        Assert.Equal(new Uri(BaseAddress + "api/ocr/analyse"), _http.LastRequest.RequestUri);
        using var body = JsonDocument.Parse(_http.LastRequestBody!);
        Assert.Equal("hello", body.RootElement.GetProperty("text").GetString());
    }

    [Fact]
    public async Task AnalyseAsync_SuccessResponse_ReturnsDeserialisedAnalysis()
    {
        _http.Respond(HttpStatusCode.OK, """{"wordCount":2,"characterCount":11,"answer":"pong","keywords":["ping"]}""");

        var result = await CreateClient().AnalyseAsync("hello world");

        Assert.NotNull(result);
        Assert.Equal(2, result.WordCount);
        Assert.Equal(11, result.CharacterCount);
        Assert.Equal("pong", result.Answer);
        Assert.Equal(["ping"], result.Keywords);
    }

    [Fact]
    public async Task AnalyseAsync_ErrorStatus_ThrowsAndLogsError()
    {
        _http.Respond(HttpStatusCode.InternalServerError);

        await Assert.ThrowsAsync<HttpRequestException>(() => CreateClient().AnalyseAsync("hello"));

        _logger.VerifyLogged(LogLevel.Error, Times.Once());
    }
}
