using CourseCrutch.Clients.Models;

namespace CourseCrutch.Clients.Services;

public interface IOcrApiClient
{
    Task<OcrAnalysisResponse?> AnalyseAsync(string text, CancellationToken cancellationToken = default);
}
