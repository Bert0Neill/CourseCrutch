namespace CourseCrutch.Application.Ocr;

public interface IOcrAnalysisService
{
    Task<OcrAnalysis> AnalyseAsync(string text, CancellationToken cancellationToken = default);
}
