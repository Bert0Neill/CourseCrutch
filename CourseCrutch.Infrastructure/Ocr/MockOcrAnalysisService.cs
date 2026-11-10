using CourseCrutch.Application.Ocr;

namespace CourseCrutch.Infrastructure.Ocr;

public sealed class MockOcrAnalysisService : IOcrAnalysisService
{
    public Task<OcrAnalysis> AnalyseAsync(string text, CancellationToken cancellationToken = default)
    {
        var words = text.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
        var keywords = words
            .Select(word => word.Trim('.', ',', ';', ':', '!', '?'))
            .Where(word => word.Length > 3)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(5)
            .ToArray();

        var analysis = new OcrAnalysis(
            words.Length,
            text.Length,
            "Mocked answer. No real analysis has run.",
            keywords);

        return Task.FromResult(analysis);
    }
}
