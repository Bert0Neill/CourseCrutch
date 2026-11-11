using System.Net.Http.Json;
using CourseCrutch.Clients.Models;
using Microsoft.Extensions.Logging;

namespace CourseCrutch.Clients.Services;

public sealed class OcrApiClient(HttpClient httpClient, ILogger<OcrApiClient> logger) : IOcrApiClient
{
    public async Task<OcrAnalysisResponse?> AnalyseAsync(string text, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Requesting OCR analysis for {CharacterCount} characters.", text.Length);

        using var response = await httpClient.PostAsJsonAsync("api/ocr/analyse", new OcrAnalysisRequest(text), cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("OCR analysis request failed with HTTP {StatusCode}.", (int)response.StatusCode);
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OcrAnalysisResponse>(cancellationToken);
    }
}
