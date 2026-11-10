namespace CourseCrutch.API.Contracts;

public sealed record OcrAnalysisRequest(string? Text);

public sealed record OcrAnalysisResponse(int WordCount, int CharacterCount, string Answer, IReadOnlyList<string> Keywords);
