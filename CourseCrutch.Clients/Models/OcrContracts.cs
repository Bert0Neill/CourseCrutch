namespace CourseCrutch.Clients.Models;

public sealed record OcrAnalysisRequest(string Text);

public sealed record OcrAnalysisResponse(int WordCount, int CharacterCount, string Summary, IReadOnlyList<string> Keywords);
