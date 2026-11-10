using System.Net.Http.Json;
using CourseCrutch.Clients.Models;

namespace CourseCrutch.Clients.Services;

public sealed class OcrApiClient(HttpClient httpClient) : IOcrApiClient
{
    public async Task<OcrAnalysisResponse?> AnalyseAsync(string text, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("api/ocr/analyse", new OcrAnalysisRequest(text), cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OcrAnalysisResponse>(cancellationToken);
    }
}
