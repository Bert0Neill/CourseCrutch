using System.Net.Http.Json;
using System.Text.Json;
using CourseCrutch.Application.Ocr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CourseCrutch.Infrastructure.Ocr;

public sealed class GroqOcrAnalysisService(HttpClient httpClient, IConfiguration configuration, ILogger<GroqOcrAnalysisService> logger) : IOcrAnalysisService
{
    private static string BuildSystemPrompt() =>
        $"You are a helpful assistant. The current UTC date and time is {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC. " +
        "Answer the user's question directly. List up to five keywords. " +
        "Reply only with JSON: {\"answer\": string, \"keywords\": string[]}.";

    public async Task<OcrAnalysis> AnalyseAsync(string text, CancellationToken cancellationToken = default)
    {
        var model = configuration["GroqAI:Model"] ?? "openai/gpt-oss-20b";

        var request = new
        {
            model,
            response_format = new { type = "json_object" },
            messages = new[]
            {
                new { role = "system", content = BuildSystemPrompt() },
                new { role = "user", content = text }
            }
        };

        logger.LogInformation("Sending {CharacterCount} characters to Groq model {Model}.", text.Length, model);

        using var response = await httpClient.PostAsJsonAsync("chat/completions", request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Groq returned HTTP {StatusCode}.", (int)response.StatusCode);
        }

        response.EnsureSuccessStatusCode();

        var completion = await response.Content.ReadFromJsonAsync<ChatCompletionResponse>(JsonSerializerOptions.Web, cancellationToken);
        var content = completion?.Choices.FirstOrDefault()?.Message.Content
            ?? throw new InvalidOperationException("Groq returned no content.");

        var payload = JsonSerializer.Deserialize<AnalysisPayload>(content, JsonSerializerOptions.Web)
            ?? throw new InvalidOperationException("Groq returned an empty analysis.");

        var wordCount = text.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).Length;

        return new OcrAnalysis(wordCount, text.Length, payload.Answer, payload.Keywords);
    }
}

internal sealed record ChatCompletionResponse(IReadOnlyList<ChatChoice> Choices);

internal sealed record ChatChoice(ChatCompletionMessage Message);

internal sealed record ChatCompletionMessage(string Content);

internal sealed record AnalysisPayload(string Answer, IReadOnlyList<string> Keywords);
