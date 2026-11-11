using System.Net;
using System.Text.Json;
using CourseCrutch.Infrastructure.Ocr;
using CourseCrutch.Tests.TestDoubles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace CourseCrutch.Tests.Infrastructure;

public sealed class GroqOcrAnalysisServiceTests
{
    private const string BaseAddress = "https://api.groq.com/openai/v1/";

    private readonly MockHttpHandler _http = new();
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly Mock<ILogger<GroqOcrAnalysisService>> _logger = new();

    private GroqOcrAnalysisService CreateService() =>
        new(_http.CreateClient(BaseAddress), _configuration.Object, _logger.Object);

    private static string Completion(string answer, params string[] keywords)
    {
        var content = JsonSerializer.Serialize(new { answer, keywords });
        return JsonSerializer.Serialize(new { choices = new[] { new { message = new { content } } } });
    }

    [Fact]
    public async Task AnalyseAsync_ValidCompletion_ReturnsAnswerAndKeywords()
    {
        _http.Respond(HttpStatusCode.OK, Completion("Paris", "capital", "France"));

        var result = await CreateService().AnalyseAsync("What is the capital of France?");

        Assert.Equal("Paris", result.Answer);
        Assert.Equal(["capital", "France"], result.Keywords);
    }

    [Theory]
    [InlineData("one two three", 3, 13)]
    [InlineData("  spaced   out  ", 2, 16)]
    public async Task AnalyseAsync_AnyText_CountsWordsAndCharacters(string text, int words, int characters)
    {
        _http.Respond(HttpStatusCode.OK, Completion("answer"));

        var result = await CreateService().AnalyseAsync(text);

        Assert.Equal(words, result.WordCount);
        Assert.Equal(characters, result.CharacterCount);
    }

    [Fact]
    public async Task AnalyseAsync_ModelConfigured_SendsConfiguredModel()
    {
        _configuration.Setup(c => c["GroqAI:Model"]).Returns("custom-model");
        _http.Respond(HttpStatusCode.OK, Completion("answer"));

        await CreateService().AnalyseAsync("text");

        using var body = JsonDocument.Parse(_http.LastRequestBody!);
        Assert.Equal("custom-model", body.RootElement.GetProperty("model").GetString());
    }

    [Fact]
    public async Task AnalyseAsync_NoModelConfigured_SendsDefaultModel()
    {
        _http.Respond(HttpStatusCode.OK, Completion("answer"));

        await CreateService().AnalyseAsync("text");

        using var body = JsonDocument.Parse(_http.LastRequestBody!);
        Assert.Equal("openai/gpt-oss-20b", body.RootElement.GetProperty("model").GetString());
    }

    [Fact]
    public async Task AnalyseAsync_AnyText_PostsUserTextToChatCompletions()
    {
        _http.Respond(HttpStatusCode.OK, Completion("answer"));

        await CreateService().AnalyseAsync("hello there");

        Assert.Equal(HttpMethod.Post, _http.LastRequest!.Method);
        Assert.Equal(new Uri(BaseAddress + "chat/completions"), _http.LastRequest.RequestUri);
        Assert.Contains("hello there", _http.LastRequestBody);
        _http.VerifySentOnce();
    }

    [Fact]
    public async Task AnalyseAsync_ErrorStatus_ThrowsAndLogsError()
    {
        _http.Respond(HttpStatusCode.Unauthorized);

        await Assert.ThrowsAsync<HttpRequestException>(() => CreateService().AnalyseAsync("text"));

        _logger.VerifyLogged(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task AnalyseAsync_NoChoices_ThrowsInvalidOperation()
    {
        _http.Respond(HttpStatusCode.OK, """{"choices":[]}""");

        await Assert.ThrowsAsync<InvalidOperationException>(() => CreateService().AnalyseAsync("text"));
    }
}
